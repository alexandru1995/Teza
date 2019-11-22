import { Component, OnInit, Input } from '@angular/core';
import { NgbActiveModal, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';


@Component({
  selector: 'app-time-piker-modal',
  templateUrl: './time-piker-modal.component.html',
  styleUrls: ['./time-piker-modal.component.css']
})
export class TimePikerModalComponent implements OnInit{
 
  @Input() inputTime;

  time;
  constructor(
    public modal: NgbActiveModal
  ) { }


  ngOnInit(): void {
    this.time = this.fromModel(this.inputTime);
  }

  save(newTime: NgbTimeStruct){
    this.modal.close(this.toModel(newTime))
  }

  private fromModel(value: string): NgbTimeStruct {
    if (!value) {
      return null;
    }
    const split = value.split(':');
    return {
      hour: parseInt(split[0], 10),
      minute: parseInt(split[1], 10),
      second: parseInt(split[2], 10)
    };
  }

  private toModel(time: NgbTimeStruct): string {
    if (!time) {
      return null;
    }
    return `${this.pad(time.hour)}:${this.pad(time.minute)}:${this.pad(time.second)}`;
  }

  private pad(i: number): string {
    return i < 10 ? `0${i}` : `${i}`;
  }

}
