using Quick;
using System.ComponentModel.DataAnnotations;

namespace SimpleMvvmDemo.ViewModel
{
    public class LoginWindowViewModel : QValidatableBase
    {
        #region Bindable Properties

        [Required(ErrorMessage = "用户名不能为空！")]
        public string UserName { get; set; }

        [Required(ErrorMessage = "密码不能为空！")]
        public string Password { get; set; }

        public bool LoginSuccess { private set; get; }

        #endregion

        #region Public functions
        public bool Login()
        {
            Validate();
            if (string.Compare("admin", UserName, true) != 0 || Password != "admin")
            {
                LoginSuccess = false;
                MsgBox.Show("用户名或密码错误！");
                return false;
            }
            LoginSuccess = true;
            return true;
        }
        #endregion
    }

}
