import { Component, input, signal } from '@angular/core';
import { Register } from '../account/register/register';
import { User } from '../../types/user';

@Component({
  selector: 'app-home',
  imports: [Register],
  templateUrl: './home.html',
  styleUrl: './home.css',
})

//这是Home组件，负责显示主页内容和管理主页相关的状态
export class Home {
  protected registerMode = signal(false);

  //然后这里用showRegisterMode方法来设置registerMode信号为true，以便在模板中显示注册表单。
  showRegisterMode(value: boolean) {
    this.registerMode.set(value);
  }
}
