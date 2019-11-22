import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { FormBuilder, FormGroup, Validators } from '@angular/forms';
import { TimePikerModalComponent } from '../time-piker-modal/time-piker-modal.component';

@Component({
  selector: 'app-add-service-modal',
  templateUrl: './add-service-modal.component.html',
  styleUrls: ['./add-service-modal.component.css']
})
export class AddServiceModalComponent implements OnInit {


  @Input() title;
  @Input() name: string = '';
  @Input() isshuer: string;
  @Input() logOutUrl: string;
  @Input() password: string = '';
  @Input() tokenExpirationTime: string;


  submitted: boolean = false;
  addServiceForm: FormGroup;

  time:string;

  constructor(
    private formBuilder: FormBuilder,
    public modal: NgbActiveModal,
    private modalService: NgbModal) { }

  get f() {
    return this.addServiceForm.controls;
  }

  ngOnInit(): void {
    this.addServiceForm = this.formBuilder.group({
      name: [this.name, [Validators.required]],
      isshuer: [this.isshuer, [Validators.required]],
      logOutUrl: [this.logOutUrl, [Validators.pattern("^(http:\/\/www\.|https:\/\/www\.|http:\/\/|https:\/\/)?[a-z0-9]+([\-\.]{1}[a-z0-9]+)*\.[a-z]{2,5}(:[0-9]{1,5})?(\/.*)?$"), Validators.required]],
      password: [this.password, [Validators.minLength(8), Validators.required]],
      tokenExpirationTime: [this.tokenExpirationTime, [Validators.required]]
    })
  }

  openTimePiker() {
    var modalRef = this.modalService.open(TimePikerModalComponent, { centered: true });
    modalRef.componentInstance.time = this.tokenExpirationTime;
    modalRef.result
      .then((rez) => {
        this.time = rez;
      })
      .catch(err => { })
  }

  save() {
    this.submitted = true;
    console.log('test')
    if(this.time != null){
      this.addServiceForm.controls.tokenExpirationTime.errors.required = false;
    }
    if (this.addServiceForm.invalid) {
      console.log(this.addServiceForm)
      return;
    }
    this.modal.close(this.addServiceForm.value)
  }
}


