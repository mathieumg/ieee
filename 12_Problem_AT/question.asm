0100 E2FE              LOOPW   0100             # Jumps until CX = 0.
0102 50                PUSH    AX               # Push AX register onto stack.
0103 5E                POP     SI               # Pop content of stack into SI register.
0104 89C3              MOV     BX,AX            # Copy content of AX into BX.
0106 D1E8              SHR     AX,1             # Shift AX by one to right, extra bit goes into CF, 0 bit added to the left.
0108 D1D1              RCL     CX,1             # Shift CX by one to the left, bit in CF comes to the right.
010A 75FA              JNZ     0106             # Jumps to instruction 0106 if AX is not equal to 0.
010C 91                XCHG    AX,CX            # Exchange the content of AX and CX.
010D BA0102            MOV     DX,0201          # Move the value 0201 in the DX register.
0110 D1C2              ROL     DX,1             # Shifts the DX register by one to the left, extra bit goes into
                                                # CF and also padded to the right.
0112 D1EB              SHR     BX,1             # Shift BX by one to right, extra bit goes into CF, 0 bit added to the left.
0114 D1D1              RCL     CX,1             # Shift CX by one to the left, bit in CF comes to the right.
0116 38DE              CMP     DH,BL            # Compares DH - BL.
0118 7EF6              JLE     0110             # Jump to instruction 0110 if DH is less or equal to BL.
011A 38D3              CMP     BL,DL            # Compares BL - DL.
011C 7E06              JLE     0124             # Jump to instruction 0124 if BL is less or equal to DL.
011E D1CB              ROR     BX,1             # Shifts the BX register by one to the right, extra bit goes into
                                                # CF and also padded to the left.
0120 D1D1              RCL     CX,1             # Shift CX by one to the left, bit in CF comes to the right.
0122 D1C3              ROL     BX,1             # Shifts the BX register by one to the left, extra bit goes into
                                                # CF and also padded to the right.
0124 28D4              SUB     AH,DL            # Puts in AH the result of AH - DL.
0126 4A                DEC     DX               # Decrements DX by 1.
0127 20D0              AND     AL,DL            # Ands all bits between AL and DL and stores the result in AL.
0129 21F2              AND     DX,SI            # Ands all bits between DX and SI and stores the result in DX.
012B 38C2              CMP     DL,AL            # Compares DL - AL. (CF is set to 1 if AL is greater than DL.)
012D 18E3              SBB     BL,AH            # Subtracks AH and CF to BL and puts result in BL.
