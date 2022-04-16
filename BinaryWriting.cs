using System;
using System.IO;
using System.Text;

namespace BinaryWriting
{
    class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("파일이 저장될 위치를 입력하세요:");
            String path = Console.ReadLine();

            try
            {
                FileStream fs = new FileStream(path, FileMode.Create);

                // 단일 바이트 쓰기
                fs.WriteByte(0x01);
                fs.WriteByte(0x23);
                fs.WriteByte(0x45);
                fs.WriteByte(0x67);
                fs.WriteByte(0x89);
                fs.WriteByte(0xAB);
                fs.WriteByte(0xCD);
                fs.WriteByte(0xEF);

                Byte[] asciiByte = Encoding.ASCII.GetBytes("ASCII string to byte!");
                Byte[] uniByte = Encoding.Unicode.GetBytes("유니코드 문자열을 바이트로 변환!");

                // ASCII 바이트 쓰기
                fs.Write(asciiByte, 0, asciiByte.Length);

                // 줄바꿈 (\r\n)
                fs.WriteByte(13);
                fs.WriteByte(10);

                // UNICODE 바이트 쓰기
                fs.Write(uniByte, 0, uniByte.Length);

                // 위치를 처음으로!
                fs.Position = 0;

                // 단일 바이트를 총 8회 읽습니다.
                for (Int32 i = 0; i < 8; i++)
                {
                    Byte readSingleByte = (Byte)fs.ReadByte();
                    Console.WriteLine("읽은 바이트 (0x{0:X4}): {1:X2}", fs.Position, readSingleByte);
                }

                Byte[] dummyAsciiByte = new Byte[asciiByte.Length];
                Byte[] dummyUniByte = new Byte[uniByte.Length];

                // 쓴 ASCII 바이트를 읽고 출력해보자!
                fs.Read(dummyAsciiByte, 0, dummyAsciiByte.Length);
                Console.WriteLine("읽은 ASCII 바이트: {0}", Encoding.ASCII.GetString(dummyAsciiByte));

                // 줄바꿈 문자(\r\n)는 건너뛴다. 
                fs.ReadByte();
                fs.ReadByte();

                // 쓴 UNICODE 바이트를 읽고 출력해보자!
                fs.Read(dummyUniByte, 0, dummyUniByte.Length);
                Console.WriteLine("읽은 UNICODE 바이트: {0}", Encoding.Unicode.GetString(dummyUniByte));

                // 작업이 끝났으니 파일을 저장하고 닫는다!
                fs.Close();

            }
            catch (Exception ex)
            {
                Console.WriteLine("작업 도중 예외가 발생했습니다!\n예외 메세지: {0}", ex.Message);
            }

            Console.WriteLine("아무 키나 누르면 종료합니다!");
            Console.ReadKey(true);
        }
    }
}
