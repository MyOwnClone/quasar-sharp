org	0xFFD10000

	mov	%r14,	0x1000
	mov	%r0,	0x0000
	livt %r0
	mov %r0,	0x3C
	mov %r1,	irq_handler
	mov	@r0,	%r1
	mov %r0,	2000
	mov	%r1,	0xFFE00111
	mov @r1,	%r0
	mov %r0,	1
	mov	%r1,	0xFFE00110
	movb @r1,	%r0
reset:
	mov	%r0,	0
iloop:
	cmp	%r0,	16
	bz	reset
	wait
	mov	%r1,	0xFFE00200
	movb @r1,	%r0
	add	%r0,	1
	br	iloop

	
irq_handler:
	mov %r0,	0xFFE00002
	mov	%r1,	0x00000000
	movb @r0,	%r1
	irtn