import { Component, Input, Output } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-contact-edit-modal',
  templateUrl: './contact-edit-modal.component.html',
  styleUrls: ['./contact-edit-modal.component.css']
})
export class ContactEditModalComponent {

  @Input() title;
  @Input() label;

  constructor(public modal: NgbActiveModal) { }
}
