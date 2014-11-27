

mov	%r14,	0xF000

main:
	mov	%r0,	0x102
	mov	%r1,	1
	bsr	outb
	mov	%r0,	msg
	bsr	puts
.inf:
	br	.inf
	rtn

outb:
	bor	%r0,	0xFFE00000
	movb	@r0,	%r1
	rtn
inb:
	bor	%r0,	0xFFE00000
	mov	%r0,	@r0
	and	%r0,	0xFF
	rtn

debug_write:
	swp	%r3,	%r0
	mov	%r2,	@r3
	and	%r2,	0xFF
	bz	.done
	mov	%r0,	0x05
	mov	%r1,	0x01
	bsr	outb
	mov	%r0,	0x06
	mov	%r1,	0
	bsr	outb
	mov	%r0,	0x05
	mov	%r1,	%r2
	bsr	outb
	mov	%r0,	0x06
	mov	%r1,	0x01
	bsr	outb
	br	debug_write
.done:
	rtn

install_int_handler:
	mul	%r1,	4
	sivt	%r2
	add	%r1,	%2
	mov	@r1,	%r0
	rtn

puts:
	mov	%r5,	%r0
.again:
	mov	%r0,	@r5
	and	%r0,	0xFF
	bz	.done
	bsr	putchar
	add	%r5,	1
	br	.again
.done:
	rtn
putchar:
	cmp	%r0,	0x0A
	bz	.newline

	mov	%r1,	position
	mov	%r2,	@r1
	mov	%r3,	%r2
	add	%r3,	0xB8000
	movb	@r3,	%r0
	add	%r3,	1
	mov	%r0,	0x0F
	movb	@r3,	%r0
	add	%r2,	2
	mov	@r1,	%r2
	
	br	.done
.newline:
	bsr	newline
	br	.done
.done:
		
	rtn

newline:
	mov	%r0,	position
	mov	%r1,	@r0
	mod	%r3,	160
	bz	.add_and_finish
.loop:
	mov	%r3,	%r1
	mod	%r3,	160
	bz	.finish
	add	%r1,	1
	br	.loop
.add_and_finish:
	add	%r1,	160
	br	.finish
.finish:
	mov	@r0,	%r1
	rtn

position:
	word	0x00000000

forecolor:
	byte	0x0F

backcolor:
	byte	0x00
msg:
	string	"Hello, Quasar 3200!"
