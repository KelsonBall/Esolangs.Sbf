using System;
using System.Collections;
using System.IO;
using System.Reflection;
using System.Reflection.Emit;
using System.Text;
using System.Threading;

namespace SbfCompiler
{
    public class Compiler
    {
        private string fileName;
        private string asmName;
        private string asmFileName;

        private AssemblyBuilder myAsmBldr;

        private FieldBuilder tape;
        private FieldBuilder pointer;
        private FieldBuilder tmp;

        private FieldBuilder a;
        private FieldBuilder b;
        private FieldBuilder c;
        private FieldBuilder d;
        private FieldBuilder e;
        private FieldBuilder f;
        private FieldBuilder g;
        private FieldBuilder h;
        private FieldBuilder clipboard;

        private TypeBuilder application;

        private MethodInfo readMI;
        private MethodInfo writeMI;

        public Compiler(string fileNameInit)
        {
            fileName = fileNameInit;
            asmName = Path.GetFileNameWithoutExtension(fileName);
            asmFileName = Path.GetFileName(Path.ChangeExtension(fileName, ".exe"));

            AssemblyName myAsmName = new AssemblyName { Name = asmName };

            myAsmBldr = Thread.GetDomain().DefineDynamicAssembly(myAsmName, AssemblyBuilderAccess.RunAndSave);
            
            writeMI = typeof(Console).GetMethod("Write", new[]{ typeof(char) });
            readMI = typeof(Console).GetMethod("Read");
            
            ModuleBuilder myModuleBldr = myAsmBldr.DefineDynamicModule(asmFileName, asmFileName);
            application = myModuleBldr.DefineType(asmName);
        }

        #region Private Methods


        /// <summary>
        /// 
        /// </summary>
        /// <param name="il"></param>
        /// <param name="count"></param>
        private void Add(ILGenerator il, int count)
        {
            switch (count)
            {
                case 0:
                    il.Emit(OpCodes.Ldc_I4_0);
                    break;

                case 1:
                    il.Emit(OpCodes.Ldc_I4_1);
                    break;

                case 2:
                    il.Emit(OpCodes.Ldc_I4_2);
                    break;

                case 3:
                    il.Emit(OpCodes.Ldc_I4_3);
                    break;

                case 4:
                    il.Emit(OpCodes.Ldc_I4_4);
                    break;

                case 5:
                    il.Emit(OpCodes.Ldc_I4_5);
                    break;

                case 6:
                    il.Emit(OpCodes.Ldc_I4_6);
                    break;

                case 7:
                    il.Emit(OpCodes.Ldc_I4_7);
                    break;

                case 8:
                    il.Emit(OpCodes.Ldc_I4_8);
                    break;

                default:
                    il.Emit(OpCodes.Ldc_I4, count);
                    break;
            }
        }

        private void IncrementPointer(ILGenerator il, int count)
        {

            //ldsfld int32 sbfout.pointer
            il.Emit(OpCodes.Ldsfld, pointer);

            //ldc.i4 1	
            Add(il, count);

            //add
            il.Emit(OpCodes.Add);

            //stsfld int32 sbfout.pointer
            il.Emit(OpCodes.Stsfld, pointer);
        }

        private void DecrementPointer(ILGenerator il, int count)
        {

            //ldsfld int32 sbfout.pointer
            il.Emit(OpCodes.Ldsfld, pointer);

            //ldc.i4 1	
            Add(il, count);

            //sub
            il.Emit(OpCodes.Sub);

            //stsfld int32 sbfout.pointer
            il.Emit(OpCodes.Stsfld, pointer);
        }

        private void Reference(ILGenerator il)
        {           
            // Add tape to stack
            il.Emit(OpCodes.Ldsfld, tape);

            // Add pointer to stack
            il.Emit(OpCodes.Ldsfld, pointer);

            // Add pointer to stack
            il.Emit(OpCodes.Ldsfld, pointer);

            // Set element to pointer
            il.Emit(OpCodes.Stelem_I4);
        }

        private void Dereference(ILGenerator il)
        {
            // Add tape to stack
            il.Emit(OpCodes.Ldsfld, tape);

            // Add pointer to stack
            il.Emit(OpCodes.Ldsfld, pointer);

            // Read cell
            il.Emit(OpCodes.Ldelem_I4);

            // Load cell to pointer
            il.Emit(OpCodes.Stsfld, pointer);
        }


        private void Plus(ILGenerator il, int count)
        {
            //ldsfld int32[] sbfout.tape
            il.Emit(OpCodes.Ldsfld, tape);

            //ldsfld int32 sbfout.pointer
            il.Emit(OpCodes.Ldsfld, pointer);

            //ldelem.i4 
            il.Emit(OpCodes.Ldelem_I4);

            //ldc.i4 1
            Add(il, count);

            //add
            il.Emit(OpCodes.Add);

            //stsfld int32 sbfout.tpointer
            il.Emit(OpCodes.Stsfld, tmp);

            //ldsfld int32[] sbfout.tape
            il.Emit(OpCodes.Ldsfld, tape);

            //ldsfld int32 sbfout.pointer
            il.Emit(OpCodes.Ldsfld, pointer);

            //ldsfld int32 sbfout.tmp	
            il.Emit(OpCodes.Ldsfld, tmp);

            //stelem.i4
            il.Emit(OpCodes.Stelem_I4);
        }

        private void Minus(ILGenerator il, int count)
        {

            //ldsfld int32[] sbfout.tape
            il.Emit(OpCodes.Ldsfld, tape);

            //ldsfld int32 sbfout.pointer
            il.Emit(OpCodes.Ldsfld, pointer);

            //ldelem.i4 
            il.Emit(OpCodes.Ldelem_I4);

            //ldc.i4 1	
            Add(il, count);

            //sub
            il.Emit(OpCodes.Sub);

            //stsfld int32 sbfout.tmp
            il.Emit(OpCodes.Stsfld, tmp);

            //ldsfld int32[] sbfout.tape
            il.Emit(OpCodes.Ldsfld, tape);

            //ldsfld int32 sbfout.pointer
            il.Emit(OpCodes.Ldsfld, pointer);

            //ldsfld int32 sbfout.tmp	
            il.Emit(OpCodes.Ldsfld, tmp);

            //stelem.i4
            il.Emit(OpCodes.Stelem_I4);
        }

        private void Double(ILGenerator il)
        {
            PushCellRef(il);

            // Load element from tape
            il.Emit(OpCodes.Ldelem_I4);

            // Push 2
            il.Emit(OpCodes.Ldc_I4_2);

            // Multiply
            il.Emit(OpCodes.Mul);

            // Set result to temp
            il.Emit(OpCodes.Stsfld, tmp);

            PushCellRef(il);

            // Push temp
            il.Emit(OpCodes.Ldsfld, tmp);

            // Set cell to temp
            il.Emit(OpCodes.Stelem_I4);
        }

        private void Halve(ILGenerator il)
        {            
            PushCellRef(il);

            il.Emit(OpCodes.Ldelem_I4);

            il.Emit(OpCodes.Ldc_I4_2);

            il.Emit(OpCodes.Div);

            il.Emit(OpCodes.Stsfld, tmp);

            PushCellRef(il);

            il.Emit(OpCodes.Ldsfld, tmp);

            il.Emit(OpCodes.Stelem_I4);


        }

        private void SwapRegister(ILGenerator il, FieldBuilder reg)
        {            
            PushCellRef(il);

            // Add tape[pointer] to stack
            il.Emit(OpCodes.Ldelem_I4);

            // Set tmp to tape[pointer]
            il.Emit(OpCodes.Stsfld, tmp);

            PushCellRef(il);

            // Load selected register to stack
            il.Emit(OpCodes.Ldsfld, reg);

            // Set tape[pointer] to reg
            il.Emit(OpCodes.Stelem_I4);

            // Load temp to stack
            il.Emit(OpCodes.Ldsfld, tmp);

            // Set register to tmp
            il.Emit(OpCodes.Stsfld, reg);
        }

        private void CopyToRegister(ILGenerator il)
        {
            PushCellRef(il);

            il.Emit(OpCodes.Ldelem_I4);

            il.Emit(OpCodes.Stsfld, clipboard);
        }

        private void PasteFromRegister(ILGenerator il)
        {
            PushCellRef(il);

            il.Emit(OpCodes.Ldsfld, clipboard);

            il.Emit(OpCodes.Stelem_I4);
        }

        private void Read(ILGenerator il)
        {

            PushCellRef(il);
            
            il.EmitCall(OpCodes.Call, readMI, null);

            il.Emit(OpCodes.Stelem_I4);
        }

        private void Write(ILGenerator il)
        {
            PushCellRef(il);

            il.Emit(OpCodes.Ldelem_I4);
            
            il.EmitCall(OpCodes.Call, writeMI, null);
        }


        private void LoopBegin(ILGenerator il, Label endLabel)
        {
            //ldsfld int32[] sbfout.tape
            il.Emit(OpCodes.Ldsfld, tape);

            //ldsfld int32 sbfout.pointer
            il.Emit(OpCodes.Ldsfld, pointer);

            //ldelem.i4 
            il.Emit(OpCodes.Ldelem_I4);

            //brfalse loop_1_end
            il.Emit(OpCodes.Brfalse, endLabel);
        }

        private void LoopEnd(ILGenerator il, Label beginLabel)
        {
            //br loop_1_start
            il.Emit(OpCodes.Br, beginLabel);
        }


        private void Zero(ILGenerator il)
        {
            //ldsfld int32[] sbfout.tape
            il.Emit(OpCodes.Ldsfld, tape);

            //ldsfld int32 sbfout.pointer
            il.Emit(OpCodes.Ldsfld, pointer);

            //ldc.i4.0
            il.Emit(OpCodes.Ldc_I4_0);

            //stelem.i4
            il.Emit(OpCodes.Stelem_I4);
        }

        private void PushCellRef(ILGenerator il)
        {
            il.Emit(OpCodes.Ldsfld, tape);

            il.Emit(OpCodes.Ldsfld, pointer);
        }

        private void Parse(ILGenerator il)
        {
            Queue q = new Queue();

            char[] data = File.ReadAllText(fileName, Encoding.UTF8).ToCharArray();
            foreach (char c in data)
            {
                if (isSbfChar(c))
                    q.Enqueue(c);
            }                

            Interpret(q, il);
        }

        private bool isSbfChar(char c)
        {
            switch (c)
            {
                case '▲':
                case '▼':
                case '²':
                case '½':
                case '→':
                case '←':
                case '¿':
                case '¡':                    
                case '≤':
                case '≥':
                case 'α':
                case 'ß':
                case 'π':
                case 'σ':
                case 'µ':
                case 'δ':
                case 'φ':
                case 'ε':
                case '↨':
                case '⌂':
                    return true;
                default:
                    return false;
            }
        }

        private int CountDuplicates(Queue q, char c)
        {
            int count = 1;
            char inst = c;

            while (c == inst && q.Count > 0)
            {
                c = (char)q.Peek();

                if (c == inst)
                {
                    c = (char)q.Dequeue();
                    ++count;
                }
            }

            return count;
        }


        private void Interpret(Queue q, ILGenerator il)
        {
            IEnumerator myEnumerator = q.GetEnumerator();

            char c;            

            while (q.Count > 0)
            {
                c = (char)q.Dequeue();

                switch (c)
                {
                    case '▲':
                        Plus(il, CountDuplicates(q, c));
                        break;
                    case '▼':
                        Minus(il, CountDuplicates(q, c));
                        break;

                    case '²':
                        Double(il);
                        break;
                    case '½':
                        Halve(il);
                        break;

                    case '→':
                        IncrementPointer(il, CountDuplicates(q, c));
                        break;
                    case '←':
                        DecrementPointer(il, CountDuplicates(q, c));
                        break;

                    case '¿':
                        Read(il);
                        break;
                    case '¡':
                        Write(il);
                        break;

                    case '≤':
                        #region Loops
                        {
                            if (q.Count > 0)
                            {
                                Queue lq = new Queue();

                                int nest = 0;
                                int startPos = q.Count;
                                bool pair = false;
                                bool zero = false;
                                bool opt = true;

                                // Find the matching ]
                                while (q.Count > 0)
                                {
                                    c = (char)q.Dequeue();

                                    if (c == '≤')
                                    {
                                        ++nest;
                                    }
                                    else if (c == '≥')
                                    {
                                        if (nest > 0)
                                        {
                                            --nest;
                                        }
                                        else
                                        {
                                            pair = true;
                                            break;
                                        }
                                    }
                                    // Check for null loop, [-], which set the current cell
                                    // to zero. There's no need to loop. Just store a zero
                                    // and move on.
                                    else if (opt && c == '▼' && (startPos - q.Count) == 1)
                                    {
                                        opt = false;

                                        // If the next character is the end of the loop...
                                        if ((char)q.Peek() == '≥')
                                        {
                                            // Eat the ] and stop the loop
                                            c = (char)q.Dequeue();
                                            zero = true;
                                            break;
                                        }
                                    }

                                    lq.Enqueue(c);
                                }

                                if (zero)
                                {
                                    Zero(il);
                                    break;
                                }

                                // If no matching ] is found in source block, report error.
                                if (q.Count != 0 && !pair)
                                {
                                    // throw System.Exception();
                                }

                                Label beginLabel = il.DefineLabel();
                                Label endLabel = il.DefineLabel();

                                il.MarkLabel(beginLabel);
                                LoopBegin(il, endLabel);

                                Interpret(lq, il);
                                LoopEnd(il, beginLabel);
                                il.MarkLabel(endLabel);
                            }
                        }
                        break;
                    #endregion

                    case 'α':
                        SwapRegister(il, a);
                        break;
                    case 'ß':
                        SwapRegister(il, this.b);
                        break;
                    case 'π':
                        SwapRegister(il, this.c);
                        break;
                    case 'σ':
                        SwapRegister(il, d);
                        break;
                    case 'µ':
                        SwapRegister(il, e);
                        break;
                    case 'δ':
                        SwapRegister(il, f);
                        break;
                    case 'φ':
                        SwapRegister(il, g);
                        break;
                    case 'ε':
                        SwapRegister(il, h);
                        break;
                    case '«':
                        CopyToRegister(il);
                        break;
                    case '»':
                        PasteFromRegister(il);
                        break;
                    case '↨':
                        Reference(il);
                        break;
                    case '⌂':
                        Dereference(il);
                        break;

                    default:
                        break;
                }
            }
        }

        #endregion

        public Type Compile()
        {            
            pointer = application.DefineField(nameof(pointer), typeof(Int32), FieldAttributes.Private | FieldAttributes.Static);         
            tape = application.DefineField(nameof(tape), typeof(Array), FieldAttributes.Private | FieldAttributes.Static);
            tmp = application.DefineField(nameof(tmp), typeof(Int32), FieldAttributes.Private | FieldAttributes.Static);
            
            a = application.DefineField(nameof(a), typeof(Int32), FieldAttributes.Private | FieldAttributes.Static);
            b = application.DefineField(nameof(b), typeof(Int32), FieldAttributes.Private | FieldAttributes.Static);
            c = application.DefineField(nameof(c), typeof(Int32), FieldAttributes.Private | FieldAttributes.Static);
            d = application.DefineField(nameof(d), typeof(Int32), FieldAttributes.Private | FieldAttributes.Static);
            e = application.DefineField(nameof(e), typeof(Int32), FieldAttributes.Private | FieldAttributes.Static);
            f = application.DefineField(nameof(f), typeof(Int32), FieldAttributes.Private | FieldAttributes.Static);
            g = application.DefineField(nameof(g), typeof(Int32), FieldAttributes.Private | FieldAttributes.Static);
            h = application.DefineField(nameof(h), typeof(Int32), FieldAttributes.Private | FieldAttributes.Static);
            clipboard = application.DefineField(
                nameof(clipboard),
                typeof(Int32),
                FieldAttributes.Private | FieldAttributes.Static);

            MethodBuilder mainBldr = application.DefineMethod(
               "main",
               MethodAttributes.Private | MethodAttributes.Static,
               typeof(Int32),
               null
               );

            ILGenerator il = mainBldr.GetILGenerator();

            // Allocate 640k of memory, the most anyone would ever need.
            il.Emit(OpCodes.Ldc_I4, 160000);
            il.Emit(OpCodes.Newarr, typeof(int));
            il.Emit(OpCodes.Stsfld, tape);

            // Set pointer to 0 (redundant?)
            il.Emit(OpCodes.Ldc_I4_0);
            il.Emit(OpCodes.Stsfld, pointer);


            Parse(il);


            // Return the value of the current cell
            il.Emit(OpCodes.Ldsfld, tape);            
            il.Emit(OpCodes.Ldsfld, pointer);            
            il.Emit(OpCodes.Ldelem_I4);            
            il.Emit(OpCodes.Ret);


            Type sbfoutType = application.CreateType();
            myAsmBldr.SetEntryPoint(mainBldr);
            myAsmBldr.Save(asmFileName);
            Console.WriteLine($"Assembly saved as '{asmFileName}'.");

            return sbfoutType;
        }
    }
}
