import { Component, computed, inject, OnInit, signal } from '@angular/core';
import { ActivatedRoute, NavigationEnd, Router, RouterEvent, RouterLink, RouterLinkActive, RouterOutlet } from '@angular/router';
import { filter } from 'rxjs';
import { Member } from '../../../types/member';
import { AgePipe } from '../../../core/pipes/age-pipe';
import { computeMsgId } from '@angular/compiler';
import { AccountService } from '../../../core/services/account-service';
import { MemberService } from '../../../core/services/member-service';

@Component({
  selector: 'app-member-detailed',
  imports: [RouterLink, RouterLinkActive, RouterOutlet, AgePipe],
  templateUrl: './member-detailed.html',
  styleUrl: './member-detailed.css',
})
export class MemberDetailed implements OnInit {
  private route = inject(ActivatedRoute); //注入ActivatedRoute服务，用于获取路由参数
  protected memberService = inject(MemberService); //注入MemberService服务，用于获取成员数据
  private accountService = inject(AccountService); //注入AccountService服务，用于获取当前用户信息
  private router = inject(Router); //注入RouterLink服务，用于导航到其他路由
  protected title = signal<string | undefined>('Profile'); //定义一个信号，用于存储成员的标题
  protected isCurrentUser = computed<boolean>(() => {
    return this.accountService.currentUser()?.id === this.route.snapshot.paramMap.get('id');
  }); //定义一个计算属性，用于判断当前用户是否是查看的成员

  ngOnInit(): void {
    this.title.set(this.route.firstChild?.snapshot?.title);

    this.router.events.pipe(
      filter(event => event instanceof NavigationEnd)
    ).subscribe({
      next: () => {
        this.title.set(this.route.firstChild?.snapshot?.title);
      }
    })
  }
}