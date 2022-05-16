import { Component, OnInit, TemplateRef } from '@angular/core';
import { Router } from '@angular/router';
import { BsModalService } from 'ngx-bootstrap/modal';
import { BsModalRef } from 'ngx-bootstrap/modal/bs-modal-ref.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { ToastrService } from 'ngx-toastr';
import { Evento } from '@app/models/Evento';
import { EventoService } from '@app/services/evento.service';
import { environment } from '@environments/environment';
import { PaginatedResult, Pagination } from '@app/models/Pagination';

@Component({
  selector: 'app-evento-lista',
  templateUrl: './evento-lista.component.html',
  styleUrls: ['./evento-lista.component.scss']
})
export class EventoListaComponent implements OnInit {

  modalRef: BsModalRef | any;
  public eventos: any = [];
  exibirImagem: boolean = true;
  filtroLista:string='';
  public eventoId = 0;
  public pagination = {} as Pagination;

  constructor(
    private eventoService: EventoService,
    private modalService: BsModalService,
    private toastr: ToastrService,
    private spinner: NgxSpinnerService,
    private router: Router

    ) { }

  ngOnInit(): void {
    this.pagination = {currentPage:1, itemsPerPage:3, totalItems:1} as Pagination;

    this.carregarEventos();
  }

  public alterarImagem(): void{
    this.exibirImagem = !this.exibirImagem;
  }


  public mostraImagem(imagemURL: string): string {
    return imagemURL !=''
      ? `${environment.apiURL}resources/images/${imagemURL}`
      : 'assets/img/cloud.png';

  }

  public carregarEventos(): void {
    this.spinner.show();
    this.eventoService.getEventos(this.pagination.currentPage,
                                  this.pagination.itemsPerPage).subscribe({
      next: (paginatedResult: PaginatedResult<Evento[]>) =>{
      this.eventos = paginatedResult.result;
      this.pagination = paginatedResult.pagination;
      },
      error: (erro:any) => {
        this.spinner.hide();
        this.toastr.error('erro ao carregar', 'erro');
      },
      complete: () => this.spinner.hide()
    });


  }
  openModal(event: any, template: TemplateRef<any>, eventoId: number): void {
    event.stopPropagation();
    this.eventoId = eventoId;
    this.modalRef = this.modalService.show(template, {class: 'modal-sm'});
  }

  public pageChanged(event: any): void {
      this.pagination.currentPage = event.page;
      this.carregarEventos();
  }

  confirm(): void {
    this.modalRef?.hide();
    this.spinner.show();
    this.eventoService.deleteEvento(this.eventoId).subscribe(
      (result: any) => {
        if (result.message == 'Deletado'){
          console.log(result);
          this.toastr.success('O evento foi deletado!', 'Deletado!');
          this.carregarEventos();
        }
      },
      (error: any) => {
        console.error(error);
        this.toastr.error(`Erro ao tentar deletar evento ${this.eventoId}`,'Erro!');
      },

  ).add(() => this.spinner.hide());


  }

  decline(): void {
    this.modalRef?.hide();
  }

  detalheEvento(id: number): void{
    this.router.navigate([`eventos/detalhe/${id}`]);
  }

}
