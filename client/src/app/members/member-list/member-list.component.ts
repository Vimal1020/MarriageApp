import { Component, OnInit } from '@angular/core';
import { Observable } from 'rxjs';
import { Member } from 'src/app/_models/Member';
import { MembersService } from 'src/app/_services/members.service';

@Component({
  selector: 'app-member-list',
  templateUrl: './member-list.component.html',
  styleUrls: ['./member-list.component.css']
})
export class MemberListComponent implements OnInit {
  member$: Observable<Member[]>;

  members: Member[];
  
  constructor(private memberService: MembersService) { }

  ngOnInit(): void {
    this.member$ = this.memberService.getMembers();
  }

}
