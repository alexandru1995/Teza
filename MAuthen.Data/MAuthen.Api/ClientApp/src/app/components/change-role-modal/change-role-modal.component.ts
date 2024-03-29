import { Component, OnInit, Input, ViewChild } from '@angular/core';
import { RoleService } from 'src/app/service/role.service';
import { Role } from 'src/app/models/role-models/role.model';
import { Subject, Observable, merge } from 'rxjs';
import { NgbTypeahead, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { debounceTime, distinctUntilChanged, filter, map } from 'rxjs/operators';
import { fadeAnimation } from '../animation';


@Component({
  selector: 'app-change-role-modal',
  templateUrl: './change-role-modal.component.html',
  styleUrls: ['./change-role-modal.component.css'],
  animations: [fadeAnimation]
})
export class ChangeRoleModalComponent implements OnInit {

  @Input() serviceId: string;
  @Input() title: string;

  @ViewChild('instance', { static: true }) instance: NgbTypeahead;
  focus$ = new Subject<string>();
  click$ = new Subject<string>();

  roles: Role[];
  error;
  constructor(
    private roleService: RoleService,
    public modal: NgbActiveModal
  ) { }

  ngOnInit() {
    this.roleService.get(this.serviceId).subscribe(roles => {
      console.log(roles);
      this.roles = roles;
    })
  }

  search = (text$: Observable<string>) => {
    const debouncedText$ = text$.pipe(debounceTime(200), distinctUntilChanged());
    const clicksWithClosedPopup$ = this.click$.pipe(filter(() => !this.instance.isPopupOpen()));
    const inputFocus$ = this.focus$;
    return merge(debouncedText$, inputFocus$, clicksWithClosedPopup$).pipe(
      map(term => (term === '' ? this.roles
        : this.roles.slice(0, 10))
      ));
  }
  save(model: any) {
    if (model.id) {
      this.modal.close(model)
    }
    else {
      this.error = "Select from existing role or add a new";
      setTimeout(() => this.hidenError(), 3000);
    }
  }
  formatter = (x: { name: string }) => x.name;
  private hidenError() {
    this.error = null
  }
}
