using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


////new


namespace MicroBaby
{
    public partial class Form1 : Form, IForm1
    {
        public Form1()
        {
            InitializeComponent();
            btnStep.Visible = false;
            btnStop.Visible = false;
            btnContinue.Visible = false;
            lblStep.Visible = false;
            lblStop.Visible = false;
            lblBreakpoint.Visible = false;


            //initialization of all the registers, memory and instruction set;
            int[] initialDataContent = new int[256] { 34,82, 21, 101, 56, 42, -85, -27, 10, 44, 69, -56, 126, 122, 98, 
            97, 43, 46, -106, 30, -90, 106, 78, -126, 62, -126, 92, -27, -122, 
            -64, 29, -92, -14, 74, 83, -13, 94, -75, -92, -99, 28, -35, 36, -119,
            18, 77, -19, 83, -40, -108, -11, -110, -24, 107, -29, -116, 42, -4, 
            69, -39, 70, 21, 35, 86, 16, 87, 12, 11, 107, -110, -93, -66, 106, 
            -9, 0, -118, 97, -21, -68, -8, 30, -7, 36, -84, -12, -34, 107, 17, 
            -102, 17, -45, 79, 56, 82, -44, -55, 113, -18, -42, -68, 40, 80, -32, 
            115, 43, -46, -118, 66, 115, 9, 116, 34, 22, 12, -90, -19, 37, -102,
            -24, 78, 48, 85, -122, -73, 0, 4, 80, 93, -105, 56, 97, 93, 8, 98, -52, 65, 105,
            -65, -70, 21, 5, 2, -34, 26, -88, -19, -1, -123, -55, 49, 109, -13, 65, 20, -96, 
            65, -28, 117, -114, 95, 84, 89, -53, 100, 11, 97, -32, 59, 10, 65, -114, -113, 
            -50, -4, -116, -99, 110, 34, 15, -85, 20, -106, -58, -55, -100, -83, -27, -38, 
            -50, -125, 75, 24, -18, 38, 101, -63, -27, 10, 99, -63, 74, 109, -40, -74, -31, 
            -55, 46, -94, -89, 88, 110, 91, 26, 13, 3, -124, -67, -104, 77, -69, 69, 39, 
            -33, 102, 81, 2, -60, 3, -42, -103, -124, -112, 75, -126, -76, -91, -71, -41, 
            119, -25, 15, -118, 60, -80, 42, 9, 71, -16, 63, -97, 33, -51, 110, 51, -21, -46};
            //InitializedataMem(initialDataContent);
            string initialCode = @"lda 5;lda #3;sta 4;add 25;add #43;addc 23;addc #4;sub 3;sub #26;subc 43;subc #4;inc;dec;and 59;and #3;or 9;or #32;inv;xor 3;xor #14;clra;clrc;cset;cmp 5;cmp #8;and 2;inc;dec;or 3;add 18;subc #2;inc;dec;and 13;and #9;or 9;or #3;inv;xor 10;xor #1;clra;clrc;cset;cmp 8;cmp #1;and 8;inc;dec;or 5;add 4;lda 5;lda #3;sta 4;add 25;add #43;addc 23;addc #4;sub 3;sub #26;subc 43; clra;clrc;cset;cmp 5;cmp #8;and 2;inc;dec;or 3;add 18;subc #2;inc;dec;and 13;and #9;or 9;or #3;inv;xor 10;xor #1;clra;clrc;cset;cmp 8;cmp #1;and 8;inc;dec;or 5;add 4;subc #4;inc;dec;and 59;and #3;or 9;or #32;inv;xor 3;xor #14;clra;clrc;cset;cmp 5;cmp #8;and 2;inc;dec;or 3;add 18;subc #2;inc;dec;and 13;and #9;or 9;or #3;inv;xor 10;xor #1;inc;dec;clra;clrc;cset;inv;inc;jmp 5";
            //InitializeInstructionMem(initialCode);
            InitializedataMem(initialDataContent);
            updateDataMem();
            updateInstructionMem();
            //initializePCArr
            RichTxtOutputInfo.Text = "";



        }



        //The overall original code line
        string originalCode = "";
        private void getTheOriginalCode()
        {
            originalCode = txtCodeRich.Text;
        }

        int[] memory = new int[256];
        //bool flag = true;
        string[] instructionReg = new string[256];
        string[] realInstructionReg = new string[256];
        int InstruCount = 0;
        byte SR = 0;
        int ACC = 0;
        int PC = 0;
        bool negtive = false;
        //debug counter initialization
        //int programCounter = 0;
        //debug code content 
        string[] debug_code;
        //debug character counter
        //int charaterCounter = 0;
        //initial arrayForInstruction
        string[] initialCodeArray;

        string[] instruction1 = { "INC", "DEC", "INV", "CLRA", "CLRC", "CSET" };
        string[] binaryCode1 = { "01001100", "01000100", "01011000", "01000000", "01001000", "01001001" };
        string[] instruction2 = { "LDA", "ADD", "ADDC", "SUB", "SUBC", "AND", "OR", "XOR", "CMP", "STA", "JMP","JMPNZ","JMPZ","JMPC","JMPNC","JMPN","JMPNN" };
        string[] binaryCode2 = { "10000010", "10000001", "01000010", "01000001", "01001010", "01001001",
                                   "01010010","01010001","01110010","01110001","01011010","01011001",
                                   "01011110","01011101","01010110","01010101","01001010","01001001",
                                   "10100010","11100000","11100010","11100001","11110000","11110001","11110011","11110101"};// added two more jumps, JMPNZ "11100010" and JMPZ "11100001"


        //InitializePCArr 

        int[] PCArr;//store the PC value for each line
        string[] codeArr;
        int[] codePointerPCArr;
        int[] loopArr;
        string[] loopNameArr;
        int[] PCtoProgramCounter;



        private void InitializePCArr(string codeForPC)
        {
            getInitialCodeArr();

            //--------------------------------------------codePointerPC and LoopArr---------------------------------------------//
            //update current start point of originalCode
            int codePointer = 0;

            //all the indexes of these two array are from 1


            //codePointerPC for pointer of original code for every pc
            //codePointerPC[value of PCArr] = start index of original code of PC
            codePointerPCArr = new int[codeArr.Length + 1];
            //loopArr[index of loopArr] = index of PCArr
            loopArr = new int[codeArr.Length + 1];
            loopNameArr = new string[codeArr.Length + 1];
            int idxLoopArr = 0;
            //--------------------------------------------codePointerPC and LoopArr--------------------------------------------//

            PCArr = new int[codeArr.Length];
            PCtoProgramCounter = new int[codeArr.Length+1];
            int tempPCcounter = 0;
            //bool firstRealCode = true;
            for (int i = 0; i < codeArr.Length; i++)
            {
                string singleLine = codeArr[i];
                string singleLineAfterTrim = singleLine.Trim();
                
                if (singleLineAfterTrim == "")
                {
                    PCArr[i] = tempPCcounter;
                }
                else {
                    if (singleLineAfterTrim.StartsWith("//"))
                    {
                        PCArr[i] = tempPCcounter;
                    }
                    //else if (singleLineAfterTrim.StartsWith("*"))
                    //{
                    //    PCArr[i] = ++tempPCcounter;

                    //}
                    //else if (singleLineAfterTrim.StartsWith("LOOP"))
                    //{
                    //    PCArr[i] = ++tempPCcounter;
                    //}
                    //else
                    //{
                    //    PCArr[i] = ++tempPCcounter;
                          
                    //}
                    else
                    {
                        PCArr[i] = ++tempPCcounter;
                        PCtoProgramCounter[tempPCcounter] = i;
                        //update pc pointer
                        codePointerPCArr[tempPCcounter] = codePointer;
                        if (singleLineAfterTrim.StartsWith("LOOP"))
                        {
                            try
                            {
                                if (singleLineAfterTrim.Length >= 6)
                                {
                                    loopArr[++idxLoopArr] = tempPCcounter;
                                    string checkDuplicate = singleLineAfterTrim.Substring(0, 5);
                                    checkLoopDup(checkDuplicate);
                                    loopNameArr[idxLoopArr] = checkDuplicate;
                                }
                                else {
                                    throw  new System.Exception("Invalid Loop line!");
                                }
                            }
                            
                                
                            catch(Exception e)
                            {
                                DateTime now = DateTime.Now;
                                RichTxtOutputInfo.Text = now.ToString() + e.Message;
                                programCounter = codeArr.Length;
                            }
                            
                        }
                    }
                
                }
                codePointer += (1 + codeArr[i].Length);
                Console.WriteLine("codepointer = " + codePointer + "   codeArr = " + codeArr[i].Length);
            }

        }

        private void checkLoopDup(String name)
        {
            try
            {
                for (int i = 0; i < loopNameArr.Length; i++)
                {
                    if (name.Equals(loopNameArr[i]))
                        throw new System.Exception(" Duplicate Loop name Error!");
                }
            }
            catch(Exception e)
            {
                DateTime now = DateTime.Now;
                RichTxtOutputInfo.Text = name + " " +  now.ToString() + e.Message;
                programCounter = codeArr.Length;
            }
        }

        private void getInitialCodeArr()
        {
            codeArr = originalCode.Split(Environment.NewLine.ToArray());
            
        }
        
        
        private void InitializedataMem(int[] initialDataContent)
        {
            for (int i = 0; i < 256; i++)
            {
                memory[i] = initialDataContent[i];
            }
            updateDataMem();
        }


        
        private void InitializeInstructionMem(string initialCode)
        {
            initialCodeArray = initialCode.Split(';');

            preProcessCode(initialCodeArray);
            clearAllSet();
            refreshRegister();
            //IRregister is not refreshed in the function above, so we need to lblIRres.Text = "";
            lblIRres.Text = "";
        }
        private void clearAllSet()
        {
            InstruCount = 0;
            SR = 0;
            ACC = 0;
            PC = 0;
            negtive = false;
            //debug counter initialization
            programCounter = 0;
            //debug character counter
            //charaterCounter = 0;
        }



        //
        int programCounter = 0;
        //
        private void btnRun_Click(object sender, EventArgs e)
        {
//            programCounter = 0;
            //getTheInitialCode for all
            setZero();
            setExecuteZero();
            getTheOriginalCode();

            RichTxtOutputInfo.Text = "";//clear RichTxtOutputInfo Error info
            programCounter = 0;//clear programCounter
            //charaterCounter = 0; // clear caracterCounter
            InitializePCArr(originalCode);
            string[] strs = originalCode.Split(Environment.NewLine.ToArray());

            preProcessCode(codeArr);

            refreshRegister();
            updateDataMem();
            updateInstructionMem();
            


        }

        private void setZero()
        {
            debugMode = false;
            SR = 0;
            ACC = 0;
            PC = 0;
            deadloop = 0;
            //programCounter = 0;
            InstruCount = 0;



        //    private void setExecuteZero()
        //{
        //    if (ACC > 255 )
        //    {
        //        ACC = ACC%255;
        //    }
        //    else if(ACC < -128) 
        //    {
        //        ACC += 128;
        //    }
        //    if (InstruCount >= 255) InstruCount = 0;
        //    if(PC > 255) PC = 0;
        //    //if (SR > 256) SR = 0;


        //}
        }

        private void updateDataMem()
        {
            try
            {
                //InitializedataMem(initialDataContent);
                GridDataMemory.Rows.Clear();
                for (int i = 0; i < 32; i++)
                {

                    String mem1 = memoryStrProcess(memory[i * 8 + 0]);
                    String mem2 = memoryStrProcess(memory[i * 8 + 1]);
                    String mem3 = memoryStrProcess(memory[i * 8 + 2]);
                    String mem4 = memoryStrProcess(memory[i * 8 + 3]);
                    String mem5 = memoryStrProcess(memory[i * 8 + 4]);
                    String mem6 = memoryStrProcess(memory[i * 8 + 5]);
                    String mem7 = memoryStrProcess(memory[i * 8 + 6]);
                    String mem8 = memoryStrProcess(memory[i * 8 + 7]);

                    GridDataMemory.Rows.Add(mem1, mem2, mem3, mem4, mem5, mem6, mem7, mem8);
                    //GridGridDataMemory.Rows.Add("1", "2", mem3, mem4, mem5, mem6, mem7, mem8);
                }
            }
            catch (Exception ex)
            {
                RichTxtOutputInfo.Text = ex.Message;
                programCounter = codeArr.Length;
            }
        }

        private void updateInstructionMem()
        {
            try
            {
                //InitializedataMem(initialDataContent);
                InstructionMemory.ReadOnly = true;
                InstructionMemory.Rows.Clear();
                int len = instructionReg.Length;
                for (int i = 0; i < len/8; i++)
                {
                    
                    string mem1 = instructionStrProcess(Convert.ToString(Convert.ToInt32(instructionReg[i * 8 + 0] , 2), 16));
                    string mem2 = instructionStrProcess(Convert.ToString(Convert.ToInt32(instructionReg[i * 8 + 1], 2), 16));
                    string mem3 = instructionStrProcess(Convert.ToString(Convert.ToInt32(instructionReg[i * 8 + 2], 2), 16));
                    string mem4 = instructionStrProcess(Convert.ToString(Convert.ToInt32(instructionReg[i * 8 + 3], 2), 16));
                    string mem5 = instructionStrProcess(Convert.ToString(Convert.ToInt32(instructionReg[i * 8 + 4], 2), 16));
                    string mem6 = instructionStrProcess(Convert.ToString(Convert.ToInt32(instructionReg[i * 8 + 5], 2), 16));
                    string mem7 = instructionStrProcess(Convert.ToString(Convert.ToInt32(instructionReg[i * 8 + 6], 2), 16));
                    string mem8 = instructionStrProcess(Convert.ToString(Convert.ToInt32(instructionReg[i * 8 + 7], 2), 16));
                  //  string mem9 = Convert.ToString(Convert.ToInt32(instructionReg[i * 8 + 0], 2), 16);

                    Console.WriteLine(instructionReg[i * 8 + 0]);
                   // InstructionMemory.Rows.Add(mem1);
                    InstructionMemory.Rows.Add(mem1 , mem2 , mem3, mem4, mem5, mem6, mem7, mem8);
                }
            }
            catch (Exception ex)
            {
                RichTxtOutputInfo.Text = ex.Message;
                programCounter = codeArr.Length;
            }
        }

        private String instructionStrProcess(String memStr)
        {
            if (memStr.Length == 1)
            {
                memStr = "0" + memStr;
                return memStr.ToUpper();
            }
            return memStr.ToUpper();
        }

        //cur memStr when mem content is negative like "fffffffff"



        private String memoryStrProcess(int mem)
        {
            string memStr = Convert.ToString(mem, 16);
            if (memStr.Length >= 2)
            {
                memStr = memStr.Remove(0, memStr.Length - 2);
            }
            else if (memStr.Length == 1)
            {
                memStr = "0" + memStr;
            }

            return memStr.ToUpper();
        }

        //major functions for dataMem update manually

        string content_old;


        private void GridDataMemory_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
            int memChoice = memory[e.RowIndex * 8 + e.ColumnIndex];
            Console.WriteLine("xxxxx" + memChoice);
            getLastEightDigits(memChoice);

            if (memChoice >= 0)
            {
                lblBinaryres.Text = Convert.ToString(memChoice % 128, 2).PadLeft(8, '0');

            }
            else 
            {
                lblBinaryres.Text = Convert.ToString(memChoice % 128, 2).Substring(24,8);
            }
            lblDecimalres.Text = memChoice.ToString();

        }

        

        private void GridDataMemory_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
           // Console.WriteLine(e.RowIndex + "  :   " + e.ColumnIndex);
           // Console.WriteLine("content = " + GridDataMemory.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
           
            content_old = (string)GridDataMemory.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;

            
        }

        private void GridDataMemory_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
           // Console.WriteLine(e.RowIndex + "  :   " + e.ColumnIndex);
            //Console.WriteLine("content = " + GridDataMemory.Rows[e.RowIndex].Cells[e.ColumnIndex].Value);
            string content = (string)GridDataMemory.Rows[e.RowIndex].Cells[e.ColumnIndex].Value;
            content = content.ToUpper();
            if (!isMemValid(content))
            {
                DateTime now = DateTime.Now;
                RichTxtOutputInfo.Text = now.ToString() + ">>\nInput Error->Please input a Hexadecimal number!";
                    GridDataMemory.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = content_old;

            }
            else
            {
                if (content.Length == 1) content = "0" + content;
                GridDataMemory.Rows[e.RowIndex].Cells[e.ColumnIndex].Value = content.ToUpper();
                memory[e.RowIndex * 8 + e.ColumnIndex] = Convert.ToInt32(content , 16);
            }
        }

        private bool isMemValid(string content)
        {
            if ((content == null) || (content.Length > 2)) return false;
            return checkDigit(content.ToCharArray());
        }

        private bool checkDigit(char[] array)
        {
            int len = array.Length;
            for(int i = 0; i < len; i++)
            {
                bool condition1 = array[i] >= '0' && array[i] <= '9';
                bool condition2 = array[i] >= 'a' && array[i] <= 'f';
                bool condition3 = array[i] >= 'A' && array[i] <= 'F';
                if (!(condition1 || condition2 || condition3))
                {
                    return false;
                }
            }
            return true;
        }

        private void preProcessCode(string[] code)
        {

            for (; programCounter < code.Length; programCounter++)
            {
                string str = code[programCounter];
                int strLen = str.Length;
                str = str.Trim();
                if (str != "")
                {
                    if (!(str.StartsWith("//")))
                    {
                        if (str.StartsWith("*"))
                        {
                            btnContinue.Visible = true;
                            startDebug();
                            StepExecution();
                            break;

                        }
                        else if(str.StartsWith("LOOP"))
                        {
                            
                            execute(str.Substring(6));
                           
                        }
                        else
                        {
                            
                            execute(str);
                           
                        }


                    }

                }
                //charaterCounter += strLen + 1;
                //programCounter++;
            }
        }

        private void refreshRegister()
        {
            lblSRres.Text = Convert.ToString(SR, 2).PadLeft(3, '0');
            if (ACC >= 0)
            {
                lblACCres.Text = Convert.ToString(ACC % 128, 2).PadLeft(8, '0');
            }

            lblPCres.Text = Convert.ToString(PC, 2).PadLeft(8, '0');
        }


        public void execute(string str)
        {
            //PC++;
            setExecuteZero();
            if (PCArr != null)
            {
                PC = PCArr[programCounter];
            }
            int num = 0;
            str = str.Trim();//delete the useless space in the head and tail
            string[] strs = System.Text.RegularExpressions.Regex.Split(str, @"\s+");//split the string by one or more spaces
            if (strs.Length == 1)
            {

                string stringBin1 = getTheBin1(strs[0]);
                if (stringBin1 != "NO-INSTRU")
                {
                    try
                    {
                        
                        lblIRres.Text = stringBin1;//exception 
                        //instructionReg[InstruCount] = stringBin1;
                        instructionReg[InstruCount++] = stringBin1;
                        instructionReg[InstruCount++] = "00000000";
                    }
                    catch (Exception e){
                        RichTxtOutputInfo.Text =  e.Message;
                        programCounter = codeArr.Length;

                    }
                    
                }
            }

            else
            {
                if (strs[0].ToUpper() == "JMPZ")
                {
                    funJMPZ(strs[1]);
                }
                else if (strs[0].ToUpper() == "JMPNZ")
                {
                    funJMPNZ(strs[1]); 
                }
                else if (strs[0].ToUpper() == "JMP")
                {
                    funJMP(strs[1]);
                }
                else if (strs[0].ToUpper() == "JMPC")
                {
                    funJMPC(strs[1]);
                }
                else if (strs[0].ToUpper() == "JMPNC")
                {
                    funJMPNC(strs[1]);
                }
                else if (strs[0].ToUpper() == "JMPN")
                {
                    funJMPN(strs[1]);
                }
                else if (strs[0].ToUpper() == "JMPNN")
                {
                    funJMPNN(strs[1]);
                }
                else 
                {
                    //lblPCres.Text = strs[1];
                    bool direct = true;
                    if (strs[1].StartsWith("#"))
                    {
                        direct = false;
                        strs[1] = strs[1].Remove(0, 1);//delete # symbols
                        if (strs[1].StartsWith("-"))
                        {
                            negtive = true;
                            strs[1] = strs[1].Remove(0, 1);
                        }
                        else
                        {
                            negtive = false;
                        }
                    }
                    try
                    {
                        num = Convert.ToInt32(strs[1]);
                        if (num < -128 || num > 255)
                        {

                            throw new System.Exception("Num out of Range!");

                        }
                        //RichTxtOutputInfo.Text = "????";

                        string IRRES = getTheBin2(strs[0], direct, num);// +numBin;//get the first half binary code of the instruction
                        if (IRRES != "NO-INSTRU")
                        {

                            lblIRres.Text = IRRES;
                            //instructionReg[InstruCount] = IRRES;
                            instructionReg[InstruCount++] = IRRES;
                            String numBin = getNumBin(num);
                            instructionReg[InstruCount++] = numBin;
                        }
                    }
                    catch (Exception e)
                    {
                        PC--;
                        //MessageBox.Show("ERROR: " + e.Message);
                        DateTime now = DateTime.Now;
                        setExecuteZero();
                        RichTxtOutputInfo.Text = now.ToString() + ">>\nERROR: " + e.Message;
                        programCounter = codeArr.Length;

                    }
 
                }

                

            }

        //    setExecuteZero();
        }
      
        private void setExecuteZero()
        {
            if (ACC > 255 )
            {
                ACC = ACC%255;
            }
            else if(ACC < -128) 
            {
                ACC += 128;
            }
            if (InstruCount >= 255) InstruCount = 0;
            if(PC > 255) PC = 0;
            //if (SR > 256) SR = 0;

            negtive = false;


        }

        //if (strs[0] == "JMPZ")
        //        {
        //            funJMPZ(strs[1]);
        //        }
        //        else if (strs[0] == "JMPNZ")
        //        {
        //            funJMPNZ(strs[1]); 
        //        }


        int deadloop = 0;
        bool debugMode = false;
        bool JMPflag = false;
        private void funJMPZ(string addr)
        {

            bool flag = false;
            
            try
            {
                for (int i = 0; i < loopArr.Length; i++)
                {
                    if (addr.Equals(loopNameArr[i]))
                    {
                        JMPflag = true;
                        flag = true;
                        if (deadloop++ == 100)
                        {
                           // Console.WriteLine("deadloopbefore = " + deadloop);

                            throw (new System.Exception("Dead Loop is happening!"));
                        }
                        int tempPC = loopArr[i];
                        if (ACC == 0)
                        {
                            programCounter = PCtoProgramCounter[tempPC]-1;
                            if (!debugMode)
                                return; //preProcessCode(codeArr);
                        }
                        else
                        {
                            JMPflag = false;
                        }
                        break;
                        
                            
                    }
                }
                if (!flag)
                {
                    deadloop = 0;
                    DateTime now = DateTime.Now;
                    RichTxtOutputInfo.Text = now.ToString() + ">>\n" + " LOOP Not Exist Error!";
                    programCounter = codeArr.Length;
                }
                
            }
            catch (Exception e)
            {

                DateTime now = DateTime.Now;
                RichTxtOutputInfo.Text = now.ToString() + ">>\nERROR: "+e.Message;
                programCounter = codeArr.Length;

                setZero();
              //  Console.WriteLine("deadloopafter = " + deadloop);
            }
                
            
        }



        
        private void funJMPNZ(string addr)
        {
            bool flag = false;

            try
            {
                for (int i = 0; i < loopArr.Length; i++)
                {
                    if (addr.Equals(loopNameArr[i]))
                    {
                        JMPflag = true;
                        flag = true;
                        if (deadloop++ == 100)
                        {
                            // Console.WriteLine("deadloopbefore = " + deadloop);
                            throw (new System.Exception("Dead Loop is happening!"));
                        }
                        int tempPC = loopArr[i];
                        if (ACC != 0)
                        {
                            programCounter = PCtoProgramCounter[tempPC]-1;
                            if (!debugMode)
                                return;// preProcessCode(codeArr);
                        }
                        else {
                            JMPflag = false;
                        }
                        break;


                    }
                }
                if (!flag)
                {
                    deadloop = 0;
                    DateTime now = DateTime.Now;
                    RichTxtOutputInfo.Text = now.ToString() + ">>\n" + " LOOP Not Exist Error!";
                    programCounter = codeArr.Length;
                }

            }
            catch (Exception e)
            {
                DateTime now = DateTime.Now;
                RichTxtOutputInfo.Text = now.ToString() + ">>\nERROR: " + e.Message;
                programCounter = codeArr.Length;


                setZero();
                //  Console.WriteLine("deadloopafter = " + deadloop);
            }
        }


        private void funJMP(String num)
        {
           
            try
            {
                bool flag = true;
                if (!num.StartsWith("#"))
                {
                    flag = false;
                    throw new System.Exception(" Offset Address incorrect Error" + num);

                }
                num = num.Substring(1);
                int addressOffset = Convert.ToInt32(num);
                int newPC = addressOffset + PC;
                if (newPC <= 0 || newPC > PCArr[PCArr.Length - 1] || addressOffset == 0)
                {
                    flag = false;
                    throw new System.Exception(" JUMP to an invalid address");
                }
                    
                JMPflag = true;
                if (deadloop++ == 100)
                {
                    // Console.WriteLine("deadloopbefore = " + deadloop);
                    flag = false;
                    throw (new System.Exception(" Dead Loop is happening!"));
                }
                programCounter = PCtoProgramCounter[newPC]-1;//
                if (!debugMode && flag)
                    return;// preProcessCode(codeArr);

                Console.WriteLine("deadloop :"+deadloop);
                
            }
            catch (Exception e)
            {
                deadloop = 0;
                DateTime now = DateTime.Now;
                RichTxtOutputInfo.Text = now.ToString() + e.Message;
                setZero();
                programCounter = PCtoProgramCounter[PCArr[PCArr.Length-1]];
            }
            
        }
        private void funJMPC(String addr)
        {

            bool flag = false;
            int mask = 4;
            try
            {
                for (int i = 0; i < loopArr.Length; i++)
                {
                    if (addr.Equals(loopNameArr[i]))
                    {
                        JMPflag = true;
                        flag = true;
                        if (deadloop++ == 100)
                        {
                            // Console.WriteLine("deadloopbefore = " + deadloop);
                            throw (new System.Exception("Dead Loop is happening!"));
                        }
                        int tempPC = loopArr[i];
                        if ((SR & mask) == mask )
                        {
                            programCounter = PCtoProgramCounter[tempPC] - 1;
                            if (!debugMode)
                                return;// preProcessCode(codeArr);
                        }
                        else {
                            JMPflag = false;
                        }
                        break;


                    }
                }
                if (!flag)
                {
                    deadloop = 0;
                    DateTime now = DateTime.Now;
                    RichTxtOutputInfo.Text = now.ToString() + ">>\n" + " LOOP Not Exist Error!";
                    programCounter = codeArr.Length;
                }

            }
            catch (Exception e)
            {
                DateTime now = DateTime.Now;
                RichTxtOutputInfo.Text = now.ToString() + ">>\nERROR: " + e.Message;
                programCounter = codeArr.Length;


                setZero();
                //  Console.WriteLine("deadloopafter = " + deadloop);
            }

        }
        private void funJMPNC(String addr)
        {
            bool flag = false;
            int mask = 4;
            try
            {
                for (int i = 0; i < loopArr.Length; i++)
                {
                    if (addr.Equals(loopNameArr[i]))
                    {
                        JMPflag = true;
                        flag = true;
                        if (deadloop++ == 100)
                        {
                            // Console.WriteLine("deadloopbefore = " + deadloop);
                            throw (new System.Exception("Dead Loop is happening!"));
                        }
                        int tempPC = loopArr[i];
                        if ((SR & mask) != mask)
                        {
                            programCounter = PCtoProgramCounter[tempPC] - 1;
                            if (!debugMode)
                                return;// preProcessCode(codeArr);
                        }
                        else {
                            JMPflag = false;
                        }
                        break;


                    }
                }
                if (!flag)
                {
                    deadloop = 0;
                    DateTime now = DateTime.Now;
                    RichTxtOutputInfo.Text = now.ToString() + ">>\n" + " LOOP Not Exist Error!";
                    programCounter = codeArr.Length;
                }

            }
            catch (Exception e)
            {
                DateTime now = DateTime.Now;
                RichTxtOutputInfo.Text = now.ToString() + ">>\nERROR: " + e.Message;
                programCounter = codeArr.Length;


                setZero();
                //  Console.WriteLine("deadloopafter = " + deadloop);
            }

        }
        private void funJMPN(String addr)
        {
            bool flag = false;
            int mask = 4;
            try
            {
                for (int i = 0; i < loopArr.Length; i++)
                {
                    if (addr.Equals(loopNameArr[i]))
                    {
                        JMPflag = true;
                        flag = true;
                        if (deadloop++ == 100)
                        {
                            // Console.WriteLine("deadloopbefore = " + deadloop);
                            throw (new System.Exception("Dead Loop is happening!"));
                        }
                        int tempPC = loopArr[i];
                        if ((SR & mask) == mask)
                        {
                            programCounter = PCtoProgramCounter[tempPC] - 1;
                            if (!debugMode)
                                return;// preProcessCode(codeArr);
                        }
                        else {
                            JMPflag = false;
                        }
                        break;


                    }
                }
                if (!flag)
                {
                    deadloop = 0;
                    DateTime now = DateTime.Now;
                    RichTxtOutputInfo.Text = now.ToString() + ">>\n" + " LOOP Not Exist Error!";
                    programCounter = codeArr.Length;
                }

            }
            catch (Exception e)
            {
                DateTime now = DateTime.Now;
                RichTxtOutputInfo.Text = now.ToString() + ">>\nERROR: " + e.Message;
                programCounter = codeArr.Length;


                setZero();
                //  Console.WriteLine("deadloopafter = " + deadloop);
            }
        }
        private void funJMPNN(String addr)
        {
            bool flag = false;
            int mask = 2;
            try
            {
                for (int i = 0; i < loopArr.Length; i++)
                {
                    if (addr.Equals(loopNameArr[i]))
                    {
                        JMPflag = true;
                        flag = true;
                        if (deadloop++ == 100)
                        {
                            // Console.WriteLine("deadloopbefore = " + deadloop);
                            throw (new System.Exception("Dead Loop is happening!"));
                        }
                        int tempPC = loopArr[i];
                        if ((SR & mask) != mask)
                        {
                            programCounter = PCtoProgramCounter[tempPC] - 1;
                            if (!debugMode)
                                return;// preProcessCode(codeArr);
                        }
                        else {
                            JMPflag = false;
                        }
                        break;


                    }
                }
                if (!flag)
                {
                    deadloop = 0;
                    DateTime now = DateTime.Now;
                    RichTxtOutputInfo.Text = now.ToString() + ">>\n" + " LOOP Not Exist Error!";
                    programCounter = codeArr.Length;
                }

            }
            catch (Exception e)
            {
                DateTime now = DateTime.Now;
                RichTxtOutputInfo.Text = now.ToString() + ">>\nERROR: " + e.Message;
                programCounter = codeArr.Length;


                setZero();
                //  Console.WriteLine("deadloopafter = " + deadloop);
            }
        }


        public string getTheBin1(string str)
        {
            switch (str.ToUpper())
            {
                case "INC":
                    funINC();
                    //RichTxtOutputInfo.Text = binaryCode1[0];
                    return binaryCode1[0];
                case "DEC":
                    funDEC();
                    return binaryCode1[1];
                case "INV":
                    funINV();
                    return binaryCode1[2];
                case "CLRA":
                    funCLRA();
                    return binaryCode1[3];
                case "CLRC":
                    funCLRC();
                    return binaryCode1[4];
                case "CSET":
                    funCSET();
                    return binaryCode1[5];
                default:
                    //MessageBox.Show(str.ToUpper().ToString() + " is not a legal instruction!");
                    DateTime now = DateTime.Now;
                    RichTxtOutputInfo.Text = now.ToString() + ">>\n"+ str.ToUpper().ToString() + " is not a legal instruction!";
                    programCounter = codeArr.Length;
                    //PC--;
                    break;


            }
            return "NO-INSTRU";
        }
        public string getTheBin2(string str, bool direct, int num)
        {
            int operand = 0;
            if (direct)
            {
                try
                {
                    if (negtive || num < 0 || num > 255)
                    {
                        PC--;
                        throw (new System.Exception());
                    }

                    else
                    {
                        operand = memory[num];

                    }
                }
                catch (Exception e)
                {
                    RichTxtOutputInfo.Text = "ERROR";
                    //MessageBox.Show("Memory Location " + num.ToString() + " is Not Available: ");
                    DateTime now = DateTime.Now;
                    RichTxtOutputInfo.Text = now.ToString() + ">>\nMemory Location " + num.ToString() + " is Not Available: ";
                    programCounter = codeArr.Length;
                }

            }
            else
            {

                try
                {

                    operand = num;
                    if (negtive)//judge if num is lower than -128; because num is always positive, only the negetive shows the sign;
                        num = -num;
                    //RichTxtOutputInfo.Text = (num+128).ToString();
                    if (num < -128 || num > 127)
                    {
                        PC--;
                        throw (new System.Exception());
                    }
                }
                catch (Exception e)
                {
                    //RichTxtOutputInfo.Text = "ERROR";
                    //MessageBox.Show(num.ToString() + " is out of Rrange ");
                    DateTime now = DateTime.Now;
                    RichTxtOutputInfo.Text = now.ToString() + ">>\n"+ num.ToString() + " is out of Rrange ";
                    programCounter = codeArr.Length;
                    return "NO-INSTRU";
                }

            }
            //Console.WriteLine("operand = " + operand);

            switch (str.ToUpper())
            {
                case "LDA":
                    funLDA(num,direct);
                    break;
                case "ADD":
                    funADD(operand);
                    break;
                case "ADDC":
                    funADDC(operand);
                    break;
                case "SUB":
                    funSUB(operand);
                    break;
                case "SUBC":
                    funSUBC(operand);
                    break;
                case "AND":
                    funAND(operand);
                    break;
                case "OR":
                    funOR(operand);
                    break;
                case "XOR":
                    funXOR(operand);
                    break;
                case "CMP":
                    funCMP(operand);
                    break;
                case "STA":
                    try
                    {
                        if (!direct)
                        {
                            throw new System.Exception(" STA only don't allow immediate numbers");
                        }
                        funSTA(num);
                    }
                    
                    catch (Exception e)
                    {
                        DateTime nowSta = DateTime.Now;
                        RichTxtOutputInfo.Text = nowSta.ToString() + num.ToString() + e.Message;
                        programCounter = codeArr.Length;

                    }

                    break;
                case "JMP":
                    funJMP(num);
                    break;
                default:
                    //MessageBox.Show(str.ToUpper().ToString() + " is not a legal instruction!");
                    DateTime now = DateTime.Now;
                    RichTxtOutputInfo.Text = now.ToString() + ">>\n"+ str.ToUpper().ToString() + " is not a legal instruction!";
                    programCounter = codeArr.Length;
                    //PC--;
                    break;

            }
            for (int i = 0; i < instruction2.Length; i++)
            {

                if (str.ToUpper() == instruction2[i])
                {
                    if (i > 8) return binaryCode2[17 + i - 8];
                    else
                    {
                        if (direct)
                        {
                            return binaryCode2[i * 2];
                        }
                        else return binaryCode2[i * 2 + 1];
                    }
                }

            }

            return "NO-INSTRU";
        }
        public string getNumBin(int number)
        {
            //int num = Convert.ToInt32(str);
            string res = Convert.ToString(number, 2).PadLeft(8, '0');
            if (res.Length > 8)
                res = res.Substring(24, 8);
            return res;
        }
        public void funINC()
        {
            ACC++;
            setSR(ACC,"INC");
        }
        public void funDEC()
        {
            ACC--;
            setSR(ACC,"DEC");
        }
        public void funINV()
        {
            ACC ^= ACC;
            setSR(ACC,"INV");
        }
        public void funCLRA()
        {
            ACC = 0;
            SR = 0;
        }
        public void funCLRC()
        {
            byte t = SR;
            t |= 3;

            if (t > 3)
                SR -= 4;
        }
        public void funCSET()
        {
            byte t = SR;
            t |= 3;

            if (t == 3)
                SR += 4;
        }
        public void funLDA(int num)
        {
        }
        public void funLDA(int num,bool direct)
        {

            try
            {
                if (!direct)
                {
                    if(num < -128 || num > 127)
                        throw (new System.Exception());
                    ACC = num;
                }
                
                else
                {
                    if (num < 0 || num > 255)
                        throw (new System.Exception());
                    else ACC = memory[num];
                }
            }
            catch (Exception e)
            {
                RichTxtOutputInfo.Text = "Error";
                //PC--;
                //MessageBox.Show("Memory Location " + num.ToString() + " is Not Available: ");
                DateTime now = DateTime.Now;
                RichTxtOutputInfo.Text = now.ToString() + ">>\nMemory Location " + num.ToString() + " is Not Available: ";
                programCounter = codeArr.Length;

            }

            if (ACC < 0)
                lblACCres.Text = Convert.ToString(ACC, 2).Substring(24, 8);
        }

        public void funADD(int num)
        {
            if (negtive)
            {
                ACC -= num;
                negtive = false;
            }
            else
                ACC += num;
            setSR(ACC,"ADD");
        }
        public void funADDC(int num)
        {
            byte t = SR;
            t |= 3;
            if (negtive)
            {
                ACC = ACC - num + (t == 3 ? 0 : 1);
                negtive = false;
            }
            else
                ACC = ACC + num + (t == 3 ? 0 : 1);
            setSR(ACC,"ADDC");
        }
        public void funSUB(int num)
        {
            if (negtive)
            {
                ACC += num;
                negtive = false;
            }
            else
                ACC -= num;
            setSR(ACC,"SUB");
        }
        public void funSUBC(int num)
        {
            byte t = SR;
            t |= 3;

            if (negtive)
            {
                ACC = ACC + num - (t == 3 ? 0 : 1);
                negtive = false;
            }
            else
                ACC = ACC - num - (t == 3 ? 0 : 1);
            setSR(ACC,"SUBC");
        }
        public void funAND(int num)
        {
            if(negtive) num = ~num + 1;
            ACC = ACC & num;
            setSR(ACC,"AND");
        }
        public void funOR(int num)
        {
            if(negtive) num = ~num + 1;
            ACC |= num;
            setSR(ACC,"OR");
        }
        public void funXOR(int num)
        {
            if(negtive) num = ~num + 1;
            ACC ^= num;
            setSR(ACC,"XOR");
        }
        public void funCMP(int num)
        {
            int res = ACC - num;
            if (res == 0)
            {
                SR &= 1;
            }
            else if (res > 0)
            {
                SR = 0;
            }
            else
            {
                SR &= 2;
            }
        }
        public void funSTA(int num)
        {
            try
            {
                if (num < 0 || num > 255)
                    throw new System.Exception(" Invalid Memory Address!");
                memory[num] = ACC;

            }
            catch (Exception e)
            {
                DateTime now = DateTime.Now;
                RichTxtOutputInfo.Text = now.ToString() + num.ToString() + e.Message;
                programCounter = codeArr.Length;

            }
            
            
        }

        public void funJMP(int num)//JUMP is still need to be fully concidered 
        {
            try
            {
                PC = (byte)(num * 2);
                if (PC > 255)
                    throw new System.Exception("Jump out of range!");
            }
            catch (Exception err)
            {
                //MessageBox.Show(err.Message);
                DateTime now = DateTime.Now;
                RichTxtOutputInfo.Text = now.ToString() + ">>\n"+ err.Message;
                programCounter = codeArr.Length;

            }

        }

        public void setSR(int ACC)//This void function is to solve Form 1 does not implement ...problem
        {
        }

        int NEGCOUNT = 0;

        public void setSR(int ACC,string fun)
        {
            if (ACC > 127)
            {
                ACC = ACC % 128;
                SR |= 4;
            }

            else if (ACC == 0)
                SR = 1;
            else if (ACC < 0)
            {

                if (ACC < -128)
                {
                    ACC %= 129;
                    if (ACC == 0)
                        ACC = -1;
                }
                ACC = Math.Abs(ACC);


                int test = ~ACC;
                if (NEGCOUNT++ == 211)
                {

                }
                lblACCres.Text = Convert.ToString(test + 1, 2).Substring(24, 8);
                switch (fun)
                {
                    case "INC": SR = 4; break;
                    case "DEC": SR = 6; break;
                }
                    
                SR = 6;//This is not right
            }

            else
            {
                SR = 0;
            }

            changeSrLedColor(SR);



        }
        private void changeSrLedColor(byte SR)
        {
            int mask = 1;

            for (int i = 0; i < 3; i++)
            {
                changeColorOfCNZ(i, (SR & mask) == mask);

                mask *= 2;
            }
        }

        private void changeColorOfCNZ(int i, Boolean on)
        {
            switch (i)
            {
                case 2: if (on)
                    {
                        panelC.BackColor = Color.Green;
                    }
                    else
                    {
                        panelC.BackColor = Color.Gray;
                    }
                    break;
                case 1: if (on)
                    {
                        panelN.BackColor = Color.Green;
                    }
                    else
                    {
                        panelN.BackColor = Color.Gray;
                    }
                    break;
                case 0: if (on)
                    {
                        panelZ.BackColor = Color.Green;
                    }
                    else
                    {
                        panelZ.BackColor = Color.Gray;
                    }
                    break;
            }
        }


        public void getLastEightDigits(int memChoice)
        {

            byte lightControl = (byte)(memChoice & (255));
            int mask = 1;

            for (int i = 0; i < 8; i++)
            {

                lightLED(i, (memChoice & mask) == mask);

                mask *= 2;
            }
        }

        public void lightLED(int i, bool on)
        {
            Console.WriteLine("i:" + i.ToString() + Convert.ToString(on));
            switch (i)
            {
                case 7: if (on)
                    {
                        picLED00.BackColor = Color.Red;
                    }
                    else
                    {
                        picLED00.BackColor = Color.Black;
                    }
                    break;
                case 6: if (on)
                    {
                        picLED01.BackColor = Color.Red;
                    }
                    else
                    {
                        picLED01.BackColor = Color.Black;
                    }
                    break;
                case 5: if (on)
                    {
                        picLED02.BackColor = Color.Red;
                    }
                    else
                    {
                        picLED02.BackColor = Color.Black;
                    }
                    break;
                case 4: if (on)
                    {
                        picLED03.BackColor = Color.Red;
                    }
                    else
                    {
                        picLED03.BackColor = Color.Black;
                    }
                    break;
                case 3: if (on)
                    {
                        picLED04.BackColor = Color.Red;
                    }
                    else
                    {
                        picLED04.BackColor = Color.Black;
                    }
                    break;
                case 2: if (on)
                    {
                        picLED05.BackColor = Color.Red;
                    }
                    else
                    {
                        picLED05.BackColor = Color.Black;
                    }
                    break;
                case 1: if (on)
                    {
                        picLED06.BackColor = Color.Red;
                    }
                    else
                    {
                        picLED06.BackColor = Color.Black;
                    }
                    break;
                case 0: if (on)
                    {
                        picLED07.BackColor = Color.Red;
                    }
                    else
                    {
                        picLED07.BackColor = Color.Black;
                    }
                    break;

            }
        }

        private void btnShowMem_Click(object sender, EventArgs e)
        {
            String MemContent = "";
            for (int i = 0; i < 256; i++)
            {
                string numInBin = Convert.ToString(memory[i], 2).PadLeft(8, '0');
                if (numInBin.Length > 8) numInBin = numInBin.Substring(24, 8);
                MemContent = MemContent + Convert.ToString(memory[i]) + "\r\n" + "Data Mem[" + i.ToString() + "]:   " + numInBin + "\t" + "\r\n";
            }
            //txtDataMemAll.Text = MemContent;
        }

        private void btnShowInsMem_Click(object sender, EventArgs e)
        {
            String MemContent = "";
            for (int i = 0; i < 256; i++)
            {
                string instru = instructionReg[i];
                if (i % 2 == 0)
                {
                    MemContent = MemContent + initialCodeArray[i / 2] + "\r\n";
                }
                MemContent = MemContent + "Instruction Mem[" + i.ToString() + "]:   " + instru + "\r\n";
            }
            //txtInsMemAll.Text = MemContent;
        }

        private void btnLoad_Click_1(object sender, EventArgs e)
        {
            loadCode();
        }

        private void loadCode()
        {
            StreamReader objStream;
            try
            {
                OpenFileDialog ofd = new OpenFileDialog();
                ofd.Filter = "*.txt (text file)|*.txt";
                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    objStream = new StreamReader(ofd.FileName);
                    //txtCodeRich.Text = "";
                    txtCodeRich.Text = objStream.ReadToEnd();
                }

            }
            catch (Exception etc)
            {
                //MessageBox.Show("An error occurred: " + etc.Message);
                DateTime now = DateTime.Now;
                RichTxtOutputInfo.Text = now.ToString() + ">>\nAn error occurred: " + etc.Message;
                programCounter = codeArr.Length;
            }
        }

        private void btnSave_Click_1(object sender, EventArgs e)
        {
            saveCode();

        }

        private void saveCode()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.Filter = "*.txt (text file)|*.txt";
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                txtCodeRich.SaveFile(sfd.FileName, RichTextBoxStreamType.PlainText);
            }

        }

        private void btnDebug_Click_1(object sender, EventArgs e)
        {
            RichTxtOutputInfo.Text = "";
            setZero();
            getTheOriginalCode();
            InitializePCArr(originalCode);
            //charaterCounter = 0;//throught debug button, start from the beginning
            programCounter = 0;
            debugMode = true;//debug flag
            startDebug();
        }


        private void startDebug()//start debugging
        {

            visibleBtns();
            debugMode = true;
            //initializeprogramCounter();

            debug_code = txtCodeRich.Text.Split(Environment.NewLine.ToArray());

        }


        private void btnStop_Click_1(object sender, EventArgs e)//stop debugging
        {
            invisibleBtns();

            //refresh codeText
            String temp = txtCodeRich.Text;
            txtCodeRich.Text = "";
            txtCodeRich.Text = temp;

            programCounter = 0;
            //--------------add from qi---------------//
            //charaterCounter = 0;
            //----------------------------------------//

            debug_code = null;
            debugMode = false;
        }
        private void invisibleBtns()
        {
            btnStep.Visible = false;
            btnStop.Visible = false;
            txtCodeRich.ReadOnly = false;
            btnContinue.Visible = false;

            lblStep.Visible = false;
            lblStop.Visible = false;
            lblBreakpoint.Visible = false;
        }

        private void visibleBtns()
        {
            btnStep.Visible = true;
            btnStop.Visible = true;
            txtCodeRich.ReadOnly = true;
            btnContinue.Visible = true;
            lblStep.Visible = true;
            lblStop.Visible = true;
            lblBreakpoint.Visible = true;
        }

        private void btnStep_Click_1(object sender, EventArgs e)
        {
            
            StepExecution();
        }

        private void StepExecution()
        {

            String temp = txtCodeRich.Text;
            txtCodeRich.Text = "";
            txtCodeRich.Text = temp;

        nextInstruction:
            if (programCounter >= debug_code.Length)
            {
                //MessageBox.Show("Debugging finished! Click Stop");
                DateTime now = DateTime.Now;
                RichTxtOutputInfo.Text = now.ToString() + ">>\nDebugging finished! Click Stop";
                //btnContinue.Visible = true;
            }
            else
            {

                string str = debug_code[programCounter];
                int strLen = str.Length;
                //string[] strs = temp.Split(Environment.NewLine.ToArray());
                //foreach (string str in strs)
                //{
                str = str.Trim();
                if (str != "")
                {
                    if (!(str.StartsWith("//")))
                    {
                        if (str.StartsWith("*"))
                            str = str.Substring(1);
                        if (str.StartsWith("LOOP"))
                        {
                            str = str.Substring(6);

                        }
                        Console.WriteLine("* step counter = " + programCounter);
                        changeCodeBackcolor(str, codePointerPCArr[PCArr[programCounter]]);

                        //simulatorInitialize();//detect if its the first time runing this simulator

                        execute(str);

                        refreshRegister();

                    }


                }
                //charaterCounter += strLen + 1;
                //if(!JMPflag)
                    programCounter++;
                JMPflag = false;
                if (str == "" || str.StartsWith("//"))
                    goto nextInstruction;

                Console.WriteLine("ACC = " + ACC);

                updateDataMem();

            }
        }


        private void changeCodeBackcolor(String str, int index)
        {
            //int index = 0;

            String temp = txtCodeRich.Text;
            txtCodeRich.Text = "";
            txtCodeRich.Text = temp;

            //Console.WriteLine("debug color index = " + index);

            txtCodeRich.Find(str, index, txtCodeRich.TextLength, RichTextBoxFinds.MatchCase);
            txtCodeRich.SelectionBackColor = Color.Yellow;
            //index = txtCodeRich.Text.IndexOf(str, index) + 1;

        }

        private void btnContinue_Click_1(object sender, EventArgs e)
        {
            debugMode = true;
            string temp = txtCodeRich.Text;
            txtCodeRich.Text = "";
            txtCodeRich.Text = temp;
            try
            {
                string str = debug_code[programCounter];
                int strLen = str.Length;
                int i = 0;
                while (!str.StartsWith("*"))
                {
                    i++;

                    //string[] strs = temp.Split(Environment.NewLine.ToArray());
                    //foreach (string str in strs)
                    //{

                    if (str.Trim() != "")//trim()
                    {
                        if (!(str.StartsWith("//")))
                        {
                            if (str.StartsWith("LOOP"))
                                str = str.Substring(6);
                            execute(str);
                            refreshRegister();
                        }


                    }

                    //-----mod from qi-----------------//
                    //charaterCounter = charaterCounter + strLen +1;

                   // charaterCounter = charaterCounter + str.Length + 1;
                    //---------------------------------//
                    programCounter++;

                    if (!(programCounter < debug_code.Length))
                        break;
                    str = debug_code[programCounter];


                }

                if (programCounter >= debug_code.Length)
                {
                    //MessageBox.Show("End of Code, Click Stop!");
                    DateTime now = DateTime.Now;
                    RichTxtOutputInfo.Text = now.ToString() + ">>\nEnd of Code, Click Stop!";
                    //btnContinue.Visible = false;
                }
                else if (str.StartsWith("*"))
                {
                    startDebug();
                    StepExecution();

                }
                Console.WriteLine("ACC = " + ACC);
                updateDataMem();

            }
            catch (Exception err)
            {
                //MessageBox.Show("End of Code, Click Stop!");
                DateTime now = DateTime.Now;
                RichTxtOutputInfo.Text = now.ToString() + ">>\nEnd of Code, Click Stop!";
                //btnContinue.Visible = false;
            }

        }
        private void gotostep()
        {
            startDebug();
            StepExecution();
        }

        private void btnNew_Click(object sender, EventArgs e)
        {
            if (txtCodeRich.Text == "")
                return;
            DialogResult dr = MessageBox.Show("Do you want to save your edit?", "Reminder", MessageBoxButtons.YesNo);

            if (dr == DialogResult.Yes)
            {
                saveCode();
            }
            txtCodeRich.Text = "";
        }

        private void btnExit_Click(object sender, EventArgs e)
        {
            this.Dispose();
        }

        private void txtCodeRich_TextChanged(object sender, EventArgs e)
        {
            showLineNo();
        }
        private void showLineNo()
        {
            //获得当前坐标信息
            Point p = new Point(0, 0);
            int crntFirstIndex = this.txtCodeRich.GetCharIndexFromPosition(p);

            int crntFirstLine = this.txtCodeRich.GetLineFromCharIndex(crntFirstIndex);

            Point crntFirstPos = this.txtCodeRich.GetPositionFromCharIndex(crntFirstIndex);

            p.Y += this.txtCodeRich.Height;

            int crntLastIndex = this.txtCodeRich.GetCharIndexFromPosition(p);

            int crntLastLine = this.txtCodeRich.GetLineFromCharIndex(crntLastIndex);
            Point crntLastPos = this.txtCodeRich.GetPositionFromCharIndex(crntLastIndex);

            //准备画图
            Graphics g = this.panelLineNumber.CreateGraphics();

            Font font = new Font(this.txtCodeRich.Font, this.txtCodeRich.Font.Style);

            SolidBrush brush = new SolidBrush(Color.Green);

            //画图开始

            //刷新画布

            Rectangle rect = this.panelLineNumber.ClientRectangle;
            brush.Color = this.panelLineNumber.BackColor;

            g.FillRectangle(brush, 0, 0, this.panelLineNumber.ClientRectangle.Width, this.panelLineNumber.ClientRectangle.Height);

            brush.Color = Color.Black;//重置画笔颜色

            //绘制行号

            int lineSpace = 0;

            if (crntFirstLine != crntLastLine)
            {
                lineSpace = (crntLastPos.Y - crntFirstPos.Y) / (crntLastLine - crntFirstLine);

            }

            else
            {
                lineSpace = Convert.ToInt32(this.txtCodeRich.Font.Size);

            }

            int brushX = this.panelLineNumber.ClientRectangle.Width - Convert.ToInt32(font.Size * 3)+7;//horizontal position

            int brushY = crntLastPos.Y + Convert.ToInt32(font.Size * 0.21f);//惊人的算法啊！！//vertical position
            for (int i = crntLastLine; i >= crntFirstLine; i--)
            {

                g.DrawString((i + 1).ToString(), font, brush, brushX, brushY);

                brushY -= lineSpace;
            }

            g.Dispose();

            font.Dispose();

            brush.Dispose();
        }
        private void txtCodeRich_VScroll_1(object sender, EventArgs e)
        {
            showLineNo();
        }
    }
}
