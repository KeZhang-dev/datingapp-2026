import { Component, inject, input, output } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { User, RegisterCreds } from '../../../types/user';
import { AccountService } from '../../../core/services/account-service';

// 2. 告诉Angular这个类是个组件，配置它
@Component({
  selector: 'app-register',    // 别人用 <app-register/> 来调用我
  imports: [FormsModule],      // 我需要用到 FormsModule
  templateUrl: './register.html', // 我的html在这里
  styleUrl: './register.css',     // 我的css在这里
})

// 3. 类本身，存数据和逻辑
export class Register {
  private accountService = inject(AccountService);
  cancelRegister = output<boolean>();        // 向父组件home发送事件
  protected creds = {} as RegisterCreds  // 存表单数据

  // 点击注册按钮触发
  register() {
    console.log(this.creds); // 打印表单数据，看看拿到什么了
    this.accountService.register(this.creds).subscribe({
      next: response => {
        console.log(response);
        this.cancel(); // 注册成功后，调用取消方法，告诉父组件关闭注册表单
      },
      error: error => console.log(error)
    })
  }

  // 点击取消按钮触发
  cancel() {
    this.cancelRegister.emit(false); // 发送事件给父组件，告诉它我取消了注册
  }
}