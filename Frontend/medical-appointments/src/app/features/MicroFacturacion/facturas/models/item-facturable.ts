export type TipoItemFactura =
  | 'Servicio'
  | 'Medicamento'
  | 'Estudio';

export interface ItemFacturable {
  tipo: TipoItemFactura;
  id: number;
  descripcion: string;
  precio: number;
}