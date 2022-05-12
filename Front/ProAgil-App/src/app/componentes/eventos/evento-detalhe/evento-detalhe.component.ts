import { CompileTemplateMetadata } from '@angular/compiler';
import { Component, OnInit, TemplateRef } from '@angular/core';
import { AbstractControl, FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { Evento } from '@app/models/Evento';
import { Lote } from '@app/models/Lote';
import { EventoService } from '@app/services/evento.service';
import { LoteService } from '@app/services/lote.service';
import { environment } from '@environments/environment';
import { BsLocaleService } from 'ngx-bootstrap/datepicker';
import { BsModalRef, BsModalService } from 'ngx-bootstrap/modal';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-evento-detalhe',
  templateUrl: './evento-detalhe.component.html',
  styleUrls: ['./evento-detalhe.component.scss']
})

export class EventoDetalheComponent implements OnInit {
  modalRef: BsModalRef| any;
  eventoId: number| any;
  evento = {} as Evento;
  form!: FormGroup;
  estadoSalvar = 'post';
  loteAtual = {id:0, nome:'', indice:0};
  imagemURL = 'assets/img/cloud.png';
  file: File| any ;

  get modoEditar(): boolean{
    return this.estadoSalvar === 'put';
  }

  get lotes(): FormArray{
    return this.form.get('lotes') as FormArray;
  }

  get f(): any{
    return this.form.controls;
  }

  get bsConfig(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY hh:mm a',
      containerClass: 'theme-default',
      showWeekNumbers: false
    };
  }

  get bsConfigLote(): any {
    return {
      isAnimated: true,
      adaptivePosition: true,
      dateInputFormat: 'DD/MM/YYYY',
      containerClass: 'theme-default',
      showWeekNumbers: false
    };
  }


  constructor(private fb: FormBuilder,
              private localeService: BsLocaleService,
              private activatedRouter: ActivatedRoute,
              private eventoService: EventoService,
              private modalService: BsModalService,
              private spinner: NgxSpinnerService,
              private toastr: ToastrService,
              private router: Router,
              private loteService: LoteService
      ) {
        this.localeService.use('pt-br');
       }

  public carregarEvento(): void{
    this.eventoId = this.activatedRouter.snapshot.paramMap.get('id');


    if (this.eventoId != null || this.eventoId != 0){
      this.estadoSalvar = 'put';

      this.spinner.show();
      this.eventoService.getEventoById(+this.eventoId).subscribe({
        next: (evento: Evento) => {
          this.evento = {...evento};
          this.form.patchValue(this.evento);
          if(this.evento.imagemURL != ''){
            this.imagemURL = environment.apiURL + this.evento.imagemURL;
          }
          this.carregarLote();
        },
        error: (error: any) => {
          this.spinner.hide();
          this.toastr.error('Erro ao tentar carregar evento','Erro!');
          console.error(error);
        },
        complete: () => {
          this.spinner.hide();
        },
      });
    }
  }

  ngOnInit(): void {
    this.carregarEvento();
    this.validation();
  }

  public validation(): void{
    this.form = this.fb.group({
      local: ['',[Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      dataEvento:  ['',Validators.required],
      tema:  ['',[Validators.required, Validators.minLength(4), Validators.maxLength(50)]],
      qtdPessoas:  ['',[Validators.required, Validators.max(120000)]],
      imagemURL:  [''],
      telefone:  ['',Validators.required],
      email:  ['',[Validators.required, Validators.email]],
      lotes: this.fb.array([])
    });
  }

  adicionarLote(): void {
    this.lotes.push(this.criarLote({id: 0} as Lote));
  }

  criarLote(lote: Lote): FormGroup {
    return this.fb.group({
      id:[lote.id],
      nome:[lote.nome, Validators.required],
      quantidade:[lote.quantidade, Validators.required],
      preco:[lote.preco, Validators.required],
      dataInicio:[lote.dataInicio],
      dataFim:[lote.dataFim]
    });
  }

  public resetForm(): void {
    this.form.reset();
  }

  public cssValidator(campoForm: FormControl | AbstractControl): any {
    return {'is-invalid': campoForm.errors && campoForm.touched}
  }


  public salvarEvento(): void{
    if (this.form.valid){
      this.spinner.show();
      if (this.estadoSalvar == 'post'){
        this.evento = {...this.form.value};
        this.eventoService.post(this.evento).subscribe(
        (eventoRetorno: Evento) => {
          this.toastr.success('Eventos salvo com sucesso.','Sucesso!');
          this.router.navigate([`eventos/detalhe/${eventoRetorno.id}`]);
        },
        (error: any) => {
          console.log(error);
          this.spinner.hide();
          this.toastr.error('Erro ao salvar evento insert',"Erro");
        },
        () => this.spinner.hide()
        );
      }
      else{
        this.evento = {id: this.evento.id, ...this.form.value};
        this.eventoService.put(this.evento).subscribe(
          () => this.toastr.success('Eventos salvo com sucesso.','Sucesso!'),
          (error: any) => {
            console.log(error);
            this.spinner.hide();
            this.toastr.error('Erro ao salvar evento update',"Erro");
          },
          () => this.spinner.hide()
          );
      }
    }
  }

  public salvarLotes(){
    this.spinner.show();
    if(this.form.controls.lotes.valid){
      this.loteService.saveLote(this.eventoId, this.form.value.lotes)
      .subscribe(
        ()=> {
          this.toastr.success('Lotes salvos com sucesso','Sucesso!');
          //this.lotes.reset();
        },
        (error: any)=> {
          this.toastr.error('Erro ao salvar lotes', 'Erro!');
          console.log(error);
        },
      ).add(()=>this.spinner.hide());

    }
  }

  public carregarLote(): void{
    this.loteService.getLotesByEventoId(this.eventoId).subscribe(
      (lotesRetorno: Lote[])=> {
          lotesRetorno.forEach(lote=>{
            this.lotes.push(this.criarLote(lote));
          });
      },
      (error)=> {
        this.eventoId.error('Erro ao tentar carregar', 'Error');
        console.error(error);
      }

    ).add(()=>this.spinner.hide());
  }

  public removerLote(template: TemplateRef<any>, indice: number): void{
    //this.loteAtual.id = this.lotes.get(indice + '.id').value;
    //this.loteAtual.nome = this.lotes.get(indice + '.nome').value;
    this.loteAtual.id = indice;
    this.modalRef = this.modalService.show(template, {class:'modal-sm'});

  }

  public confirmDeleteLote(): void{
    this.modalRef.hide();
    this.spinner.show();
    this.loteService.deleteLote(this.eventoId, this.loteAtual.id).subscribe(
      ()=>{
        this.toastr.success('Lote deletado com sucesso.','Sucesso');
        this.lotes.removeAt(this.loteAtual.indice);
      },
      (error: any)=>{
        this.toastr.error('Erro ao tentar deletar o lote', 'Erro');
        console.log(error);
      }
    ).add(()=>this.spinner.hide());
  }
  public declineDeleteLote(): void{
    this.modalRef.hide();
  }

  public onFileChange(ev: any): void{
    const reader = new FileReader();
    reader.onload = (event: any) => this.imagemURL = event.target.result;
    this.file = ev.target.files;
    reader.readAsDataURL(this.file[0]);
    this.uploadImage();
  }

  uploadImage(): void{
    this.spinner.show();
    this.eventoService.postUpload(this.eventoId, this.file).subscribe(
      () => {
        this.carregarEvento();
        this.toastr.success('Imagem atualizada com sucesso','Sucesso"');
      },
      (error: any) => {
        this.toastr.error('Erro ao tentar carregar imagem','Erro!');
        console.log(error);
      }

    ).add(()=>this.spinner.hide());
  }

}
