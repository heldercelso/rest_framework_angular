import { Component, OnInit } from '@angular/core';
import { Client } from 'src/app/models/client.model';
import { ClientService } from 'src/app/services/client.service';
import { SegmentService } from 'src/app/services/segment.service';

@Component({
  selector: 'app-client',
  templateUrl: './client.component.html',
  styleUrls: ['./client.component.css']
})

export class ClientComponent implements OnInit {
  client: Client = {
    cpf: '',
    name: '',
    segment: '',
    //segments: ['Teste1', 'Teste2', 'Teste3'],
  };

  submitted = false;
  error_msg: string = "";

  constructor(private clientService: ClientService) { }
  // segment = ['Teste1'];
  // selected = '';
  segments: string[] = [];//this.getAll();
  // selectedItem = '';
  // default_segment = '';
  // segment = {Id: 1, name: 'Private'};
  // segments = [
  //   {Id: 1, name: 'Private'},
  //   {Id: 2, name: 'Varejo'},
  //   {Id: 3, name: 'Personnalite'},
  // ];
  // refreshList(res: any): void {
  //   for(var i = 1; i <= res.length; i++) {
  //     this.client.segments?.push(res[i]);
  //   }
  // }

  // selectOption() {
  //   // console.log(this.default_segment);
  //   console.log(this.selectedItem);
  //   this.client.segment = this.selectedItem;
  //   console.log(this.client.segment);
  // }
  getAll(): any {
    this.clientService.getAllSegments()
      .subscribe({
        next: (res) => {
          //console.log(res);
          let idx=1;
          res.forEach(element => {
            // this.selected = 'Teste3';
            if (element.name != '') {
              this.segments.push(element.name!);
            }
            
            // this.segments.push({id: idx, name: element.name!});
            idx+=1
          });
          // this.selectedItem = this.segments[0];
          // console.log(this.segments);
          //this.refreshList(res);
          // this.segments = ['Teste1', 'Teste2'];
        },
        error: (e) => console.error(e)
      });
  }
  ngOnInit(): void {
    this.getAll();
  }
  saveClient(): void {
    const data = {
      name: this.client.name,
      cpf: this.client.cpf,
      segment: this.client.segment,
    };
    console.log(this.client.segment);
    console.log(data);
    this.clientService.create(data)
      .subscribe({
        next: (res) => {
          console.log(res);
          this.submitted = true;
        },
        error: (e) => {
          console.error(e);
          if (e.error.errors) {
            for (let err in e.error.errors) {
              if (!err.startsWith("$")) {
                this.error_msg = e.error.errors[err];
                return;
              }
              if (e.error.errors["$.cpf"]) {
                this.error_msg = "Campo Cpf inválido.";
                return;
              }
            }
          }
          if (typeof e.error === 'string' || e.error instanceof String) {
            this.error_msg = "Cliente com este Cpf já foi cadastrado anteriormente.";
          } else {
            for (let err in e.error) {
              this.error_msg = e.error[err];
              return;
            }
          }

          if (this.error_msg == "")
            this.error_msg = "Erro desconhecido. Confira os campos e tente novamente.";
        }
      });
      this.error_msg = "";
  }
  newClient(): void {
    this.submitted = false;
    this.client = {
      name: '',
      cpf: '',
      segment: '',
    };
  }
}
