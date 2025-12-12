export interface Genero {
  id: number;
  descricao: string;
  totalLivros?: number;
}

export type GeneroPayload = Pick<Genero, "descricao">;