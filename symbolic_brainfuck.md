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