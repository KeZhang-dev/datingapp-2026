import { HttpClient } from '@angular/common/http';
import { Component, inject, OnInit, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { lastValueFrom } from 'rxjs';
import { Nav } from "../layout/nav/nav";
import { AccountService } from '../core/services/account-service';
import { Home } from "../features/home/home";
import { User } from '../types/user';

//告诉Angular这是一个组件，并提供了组件的元数据，包括选择器、模板和样式等信息。
@Component({
  selector: 'app-root',
  imports: [Nav, Home],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
//这里实现了OnInit接口，表示组件在初始化时会执行ngOnInit方法
//它的作用是当组件被创建并插入到DOM中时，执行一些初始化逻辑，比如获取数据或设置状态。
export class App implements OnInit {
  private accountService = inject(AccountService);
  private http = inject(HttpClient); //angular的依赖注入机制，注入HttpClient服务
  protected title = 'Dating App';
  protected members = signal<User[]>([]) //数据在这里，这里会从后端API获取成员列表，并将其存储在members信号中，以便在组件模板中使用。

  //然后在ngOnInit方法中，我们从本地存储中获取当前用户信息，并设置到AccountService的currentUser信号中。
  // 同时，我们还调用getMembers方法从后端API获取成员列表，并将其设置到members信号中，以便在组件模板中使用。
  //它的目的是在应用启动时就获取必要的数据和状态，以便用户能够立即看到相关信息，而不需要等待用户交互。
  async ngOnInit() {
    this.members.set(await this.getMembers());
    this.setCurrentUser();
  }

  //这个是一个辅助方法，用于从localStorage中获取用户信息，并将其设置到AccountService的currentUser信号中，
  // 以便在应用中全局访问当前用户状态。
  //它的目的是确保在应用启动时，如果用户之前已经登录过，那么他们的登录状态能够被正确恢复，从而提供更好的用户体验。
  setCurrentUser() {
    const userString = localStorage.getItem('user');
    if (!userString) return;
    const user = JSON.parse(userString);
    this.accountService.currentUser.set(user);
  }

  //接下来这里将getMembers方法定义为一个异步方法，使用HttpClient从后端API获取成员列表，并返回一个Promise。
  //它的目的是提供一个方便的方式来获取成员数据，并且在组件初始化时调用这个方法来填充members信号，以便在模板中显示成员信息。
  async getMembers() {
    try {
      return lastValueFrom(this.http.get<User[]>('https://localhost:5001/api/members'));
    } catch (error) {
      console.log(error);
      throw error;
    }
  }
}
