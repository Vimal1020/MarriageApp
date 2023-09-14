import { Component, Input, OnInit } from '@angular/core';
import { ToastrService } from 'ngx-toastr';
import { Member } from 'src/app/_models/Member';
import { UserParams } from 'src/app/_models/userParams';
import { MembersService } from 'src/app/_services/members.service';
import { PresenceService } from 'src/app/_services/presence.service';
import { MemberEditComponent } from '../member-edit/member-edit.component';

@Component({
  selector: 'app-member-card',
  templateUrl: './member-card.component.html',
  styleUrls: ['./member-card.component.css']
})
export class MemberCardComponent implements OnInit {
  @Input() member:Member;

  constructor(private memberService: MembersService, 
    private toastr: ToastrService, public presence: PresenceService) { }

  ngOnInit(): void {
  }

  addLike(member: Member){
    this.memberService.addLike(member.username).subscribe(() => {
      this.toastr.success("You have liked " + member.knownAs); 
      this.memberService.memberCache.get(Object.values);
    })
  }
  addDislike(member: Member){
    this.memberService.addDislike(member.username).subscribe(() => {
      this.toastr.success("You have disliked " + member.knownAs);
      window.location.reload();
    })
  }

}
