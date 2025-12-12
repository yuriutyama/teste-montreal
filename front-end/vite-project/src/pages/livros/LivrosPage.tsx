import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import type { ApiResponse } from "../../model/Api";
import type { Livro } from "../../model/Livro";
import api from "../../services/api";

export default function LivrosPage() {
  const [livros, setLivros] = useState<Livro[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const loadLivros = async () => {
    setLoading(true);
    setError("");
    try {
      const { data } = await api.get<ApiResponse<Livro[]>>("/livros");
      setLivros(data.data ?? []);
    } catch (err) {
      console.error(err);
      setError("Não foi possível carregar os livros.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadLivros();
  }, []);

  const handleDelete = async (id: number) => {
    const confirmed = window.confirm("Excluir este livro?");
    if (!confirmed) return;

    try {
      await api.delete(`/livros/${id}`);
      setLivros((current) => current.filter((l) => l.id !== id));
    } catch (err) {
      console.error(err);
      setError("Erro ao excluir livro.");
    }
  };

  return (
    <section className="card">
      <div className="actions">
        <div className="section-title">
          <h2>Livros</h2>
          <span className="pill">{livros.length}</span>
        </div>
        <div style={{ display: "flex", gap: "0.5rem" }}>
          <Link className="btn secondary" to="/autores">
            Autores
          </Link>
          <Link className="btn secondary" to="/generos">
            Gêneros
          </Link>
          <Link className="btn primary" to="/livros/novo">
            Novo livro
          </Link>
        </div>
      </div>

      {error && <p className="status">{error}</p>}

      <table className="table">
        <thead>
          <tr>
            <th>Título</th>
            <th>Autor</th>
            <th>Gênero</th>
            <th style={{ width: 200 }}>Ações</th>
          </tr>
        </thead>
        <tbody>
          {loading ? (
            <tr>
              <td colSpan={4}>Carregando...</td>
            </tr>
          ) : livros.length ? (
            livros.map((livro) => (
              <tr key={livro.id}>
                <td>{livro.titulo}</td>
                <td>{livro.autorNome}</td>
                <td>{livro.generoDescricao}</td>
                <td>
                  <div style={{ display: "flex", gap: "0.5rem" }}>
                    <Link
                      to={`/livros/${livro.id}/editar`}
                      className="btn secondary"
                    >
                      Editar
                    </Link>
                    <button
                      className="btn danger"
                      onClick={() => handleDelete(livro.id)}
                    >
                      Excluir
                    </button>
                  </div>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={4}>Nenhum livro cadastrado.</td>
            </tr>
          )}
        </tbody>
      </table>
    </section>
  );
}

