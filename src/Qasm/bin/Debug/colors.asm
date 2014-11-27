
main:


reset:
	mov	%r0,	0xB8000
resetcol:
	mov	%r1,	0xF0
loop:

	cmp	%r0,	0xB8FA1
	bz	reset
	mov	%r2,	65
	movb	@r0,	%r2
	
	add	%r0,	1

	movb	@r0,	%r1

	cmp	%r1,	0xFF

	bz	resetcol

	add	%r0,	1
	add	%r1,	1

	br	loop

	rtn
