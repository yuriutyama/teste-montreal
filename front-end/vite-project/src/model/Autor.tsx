export interface Autor {
  id: number;
  nome: string;
  totalLivros?: number;
}

export type AutorPayload = Pick<Autor, "nome">;