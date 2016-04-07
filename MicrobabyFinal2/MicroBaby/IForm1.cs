namespace MicroBaby
{
    public interface IForm1
    {
        void execute(string str);
        void funADD(int num);
        void funADDC(int num);
        void funAND(int num);
        void funCLRA();
        void funCLRC();
        void funCMP(int num);
        void funCSET();
        void funDEC();
        void funINC();
        void funINV();
        void funJMP(int num);
        void funLDA(int num);
        void funOR(int num);
        void funSTA(int num);
        void funSUB(int num);
        void funSUBC(int num);
        void funXOR(int num);
        void getLastEightDigits(int memChoice);
        string getNumBin(int number);
        string getTheBin1(string str);
        string getTheBin2(string str, bool direct, int num);
        void lightLED(int i, bool on);
        void setSR(int ACC);
    }
}