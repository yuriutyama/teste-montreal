export interface Livro {
  id: number;
  titulo: string;
  autorId: number;
  autorNome: string;
  generoId: number;
  generoDescricao: string;
}

export interface LivroPayload {
  titulo: string;
  autorId: number;
  generoId: number;
}
