import { Component, inject, OnInit, signal } from '@angular/core';
import { Router, RouterOutlet } from '@angular/router';
import { Nav } from "../layout/nav/nav";


//告诉Angular这是一个组件，并提供了组件的元数据，包括选择器、模板和样式等信息。
@Component({
  selector: 'app-root',
  imports: [Nav, RouterOutlet],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
//这里实现了OnInit接口，表示组件在初始化时会执行ngOnInit方法
//它的作用是当组件被创建并插入到DOM中时，执行一些初始化逻辑，比如获取数据或设置状态。
export class App {
  protected router = inject(Router); //注入Router服务，用于导航和路由管理
}
