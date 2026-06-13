import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root',
})
export class ToastService {

  constructor() {
    this.createToasContainer(); //在服务的构造函数中调用createToasContainer方法，确保在服务实例化时就创建好toast容器，以便后续使用。
  }

  private createToasContainer() { //这里创建一个容器放在页面的底部，用于显示toast消息
    if (!document.getElementById('toast-container')) {
      const container = document.createElement('div'); //创建一个div元素作为toast容器
      container.id = 'toast-container'; //设置容器的id属性为'toast-container'，以便后续通过id来获取这个容器
      container.className = 'toast toast-bottom toast-end'; //设置容器的class属性，使用一些预定义的样式类来控制toast的位置和样式
      document.body.appendChild(container); //这里其实就是把容器放到页面的底部了，因为它是直接添加到body元素中的
    }
  }

  //从这开始就是创建toast元素并显示的逻辑了，
  // 这个方法接受三个参数：meassage是要显示的消息内容，alertClass是控制toast样式的类名，duration是toast显示的持续时间，默认为5000毫秒（5秒）
  private createToastElement(meassage: string, alertClass: string, duration = 5000) {
    const toastContainer = document.getElementById('toast-container');
    if (!toastContainer) return;

    const toast = document.createElement('div'); //创建一个div元素作为toast
    toast.classList.add('alert', alertClass, 'shadow-lg'); //设置toast的class属性，自定义样式，然后下面是innerHTML是设置toast的内容，这里使用了一个span来显示消息内容，还有一个按钮用于关闭toast
    toast.innerHTML = ` 
    <span>${meassage}</span>
    <button class="ml-4 btn btn-sm btn-ghost">X</button>`

    toast.querySelector('button')?.addEventListener('click', () => { //这个叫事件委托，给toast button加一个事件监听器，当点击这个按钮时，就会把这个toast从页面中移除掉
    })


    toastContainer.append(toast); //然后这个append就是把这个toast元素添加到toast容器中，这样它就会显示在页面上了

    setTimeout(() => { //setTimeout是定时器函数，它可以在指定时间来执行函数，然后时间到了就会把这个toast从页面中移除掉，这样就实现了toast自动消失的效果
      if (toastContainer.contains(toast)) {
        toastContainer.removeChild(toast);
      }
    }, duration);
  }

  //下面是一些公共方法，分别用于显示不同类型的toast消息，比如成功、错误、警告和信息等。
  // 这些方法都是调用createToastElement方法来创建和显示toast的，并且传入不同的样式类来控制toast的外观。
  success(message: string, duration?: number) {
    this.createToastElement(message, 'alert-success', duration);
  }

  error(message: string, duration?: number) {
    this.createToastElement(message, 'alert-error', duration);
  }

  warning(message: string, duration?: number) {
    this.createToastElement(message, 'alert-warning', duration);
  }

  info(message: string, duration?: number) {
    this.createToastElement(message, 'alert-info', duration);
  }
}
