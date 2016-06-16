# Runtime environment
The runtime defines several values and structures that are used to execute SBF programs.
 * tape - A very large array of integers
 * pointer - An integer index that points to a value in the tape
 * tmp - An integer used internally to evaluate SBF commands
 * Register Stack - A stack of sets of 9 integers that stores register values.
    * Registers α, ß, .. ε - Integers that can be swapped immediately with any cell value.
    * Clipboard - A special register that is copied to/from instead of swapping with a cell.
 * Scope Stack- A stack of integer pairs that define boundaries for the pointer. 

# Pointer behavior
The language manipulates a pseudo-infinite tape of values by moving a pointer that referencdes a particular cell along the tape
and performing operations on the value stored in the cell.

## Tape
Real world implmentations of Symbolic Brainfuck manipulate memory on a tape that consists of 160,000 32 bit integers numbered 0 
through 159,999. The pointer is initialized to 0. The pointer bounds are initialized to [0, 160000).

If the program attempts to move the pointer outside of the bounds the inner loop will be terminated instead. If there is no
loop an exception will be thrown. 

## Procedures
### Declaring and calling procedures
Procedures are declared by using the function symbol (ƒ) and terminated with the terminal symbol (■). When a procedure is declared
the pointers position used as the procedure index. The value of the cell under the pointer is interpreted as the 
procedures scope length. 

To call a procedure set the pointer to the index of the procedure and use the ¶ command.
When a procedure is called the pointer is incremented, the register stack is duplicated, and the new pointer bounds are pushed 
to the bounds stack. 
A procedures bounds are defined as [index, index + length) where index is the location of the procedure
and length is the length of the procedures scope. 

### Scopes and nested procedures.
Attempting to push pointer bounds that reach outside the current pointer bounds should throw an exception.
Attempting to move the pointer outside of the bounds will break or throw an exception. When a procedure terminates its scope 
bounds are popped from the stack. 

Feeding arguments to a procedure is done by setting cells inside the procedure scope before calling it and then reading values
from the scope after calling it. 

Currently procedures can not be defined from within the definition of another procedure. To allow procedures to call other procedures
the callee must exist completely within the callers scope, and is therefore considered a nested procedure.

#### Consequences of procedure scope and nesting
All procedures in the language are restricted from producing side effects since they can not effect anything outside of their scope. 
As a result of not being able to call procedures outside of the current scope all procedures are inherently organized in a tree structure.
Since a procedures scope includes its index it is possible for a procedure to call itself.


#Commands

|BrainFuck | Symbolic Brainfuck | Character Code | Description |
| - | -   | -    | -    |

##Arithmetic

|BF | SBF | CHAR | DESC |
| - | -   | -    | -    |
|+  | ▲   |30    |Increment cell|
|-  | ▼   |31    |Decrement cell|
|   | ²   |253   |Double cell|
|   | ½   |171   |Halve cell|

##Addressing

|BF | SBF | CHAR | DESC |
| - | -   | -    | -    |
|>  | →   |26    |Increment pointer |
|<  | ←   |27    |Decrement pointer |
|   | ↨   |23    |Set cell to pointer (Reference) |
|   | ⌂   |127   |Set pointer to cell (Dereference) |

##I/O

|BF | SBF | CHAR | DESC |
| - | -   | -    | -    |
|.  | ¡   |173   |Write cell to standard out |
|,  | ¿   |168   |Read from standard in to cell |

##Flow

|BF | SBF | CHAR | DESC |
| - | -   | -    | -    |
|[  | ≤   |243   |Jump to matching ≥ if cell is 0 |
|]  | ≥   |242   |Jump to matching ≤|

##Memory

|BF | SBF | CHAR | DESC |
| - | -   | -    | -    |
   || «   |174   |Copy cell value to clipboard|
   || »   |175   |Copy clipboard value to cell|
   || α   |224   |Swap register α with cell |
   || ß   |225   |Swap register ß with cell |
   || π   |227   |Swap register π with cell |
   || σ   |229   |Swap register σ with cell |
   || µ   |230   |Swap register µ with cell |
   || δ   |235   |Swap register δ with cell |
   || φ   |237   |Swap register φ with cell |
   || ε   |238   |Swap register ε with cell |

##Procedure
|BF | SBF | CHAR | DESC |
| - | -   | -    | -    |
   || ¶   | 20   | Push [pointer, pointer + cell) to scope stack, increment pointer, jump execution to procedure |
   || ƒ   | 159  | Start a procedure definition |
   || ■   | 254  | End a procedure definition   |

##Bitwise Operations - NON STANDARD

|BF | SBF | CHAR | DESC |
| - | -   | -    | -    |
   ||¬    |172    |Binary NOT cell |
   ||∨    |U+2228 |Binary OR cell and next cell| 
   ||∧    |U+2227 |Binary AND cell and next cell|
   ||⊕   |U+2295 |Binary XOR cell and next cell|
