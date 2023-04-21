org 30000

set 0,c
set 1,c
set 2,c
set 3,c
set 4,c
set 5,c
set 6,c
set 7,c

res 0,c
res 1,c
res 2,c
res 3,c
res 4,c
res 5,c
res 6,c
res 7,c

loop:

ld c,85

bit 0,c
bit 1,c
bit 2,c
bit 3,c
bit 4,c
bit 5,c
bit 6,c
bit 7,c

ld c,170

bit 0,c
bit 1,c
bit 2,c
bit 3,c
bit 4,c
bit 5,c
bit 6,c
bit 7,c

jr loop