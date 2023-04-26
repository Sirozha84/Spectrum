org 30000
ld hl,30040
ld (hl),45
inc hl
ld (hl),65
inc hl
ld (hl),145

ld ix, 30040

add a,(ix+0) 
add a,(ix+1) 
add a,(ix+2) 

loop:

jr loop