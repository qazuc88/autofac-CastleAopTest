using Castle.DynamicProxy;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extras.DynamicProxy;
namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var container = new Autofac.ContainerBuilder();

            container.RegisterType<LoginService>().As<ILoginService>().EnableInterfaceInterceptors().InterceptedBy(typeof(LogInterceptor), typeof(Log2Interceptor));
            //container.RegisterAssemblyTypes("ConsoleApplication1");
            container.RegisterType<LogInterceptor>();
            container.RegisterType<Log2Interceptor>();
            var ioc = container.Build();
            var loginservice = ioc.Resolve<ILoginService>();
            loginservice.Login("admin", "admin");
            loginservice.Login("admin", "admin1");
            loginservice.Logout("admin");
            Console.ReadKey();

        }
    }
    public interface ILoginService
    {
        object Login(string username, string password);
        object Logout(string username);
    }
    public class LoginService : ILoginService
    {
        public Object Login(string username, string password)
        {
            StringBuilder error = new StringBuilder();
            if (username == "admin" && password == "admin")
            {
                error.Append("登录成功");
            }
            else
            {
                error.Append("用户名或密码错误");
            }
            return new Message( error.ToString());
        }
        public Object Logout(string username)
        {
            StringBuilder error = new StringBuilder();
            error.AppendFormat("{0}已退出登录", username);
            return new Message( error.ToString());
        }
    }
    public class Message
    {
        public Message(string msg)
        {
            this.Msg = msg;
        }
        public string Msg { get; set; }
    }
    public class LogInterceptor : IInterceptor
    {

        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;
            Console.WriteLine(string.Format("log1Entered Method:{0}, Arguments: {1},result:{2}", methodName, string.Join(",", invocation.Arguments), invocation.ReturnValue));
            invocation.Proceed();
            Console.WriteLine(string.Format("log1Sucessfully executed method:{0},result{1}", methodName, invocation.ReturnValue));
        }
    }
    public class Log2Interceptor : IInterceptor
    {

        public void Intercept(IInvocation invocation)
        {
            var methodName = invocation.Method.Name;
            Console.WriteLine(string.Format("log2Entered Method:{0}, Arguments: {1},result:{2}", methodName, string.Join(",", invocation.Arguments), invocation.ReturnValue));
            invocation.Proceed();
            Console.WriteLine(string.Format("log2Sucessfully executed method:{0},result{1}", methodName, invocation.ReturnValue));
        }
    }
}
