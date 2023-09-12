import { Component, Input } from '@angular/core';
import { BsModalRef } from 'ngx-bootstrap/modal';
import { User } from 'src/app/_models/user';
import { EventEmitter } from 'stream';

@Component({
  selector: 'app-roles-modal',
  templateUrl: './roles-modal.component.html',
  styleUrls: ['./roles-modal.component.css']
})
export class RolesModalComponent {
  @Input() updateSelectedRoles = new EventEmitter();
  user:User;
  roles:any[];

  constructor(public bsModalRef :BsModalRef){}

  updateRoles()
  {
    this.roles.forEach(role => {
      this.updateSelectedRoles.emit(role);
    });
    this.bsModalRef.hide();
  }

}
