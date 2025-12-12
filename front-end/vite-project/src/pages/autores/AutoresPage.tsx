import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import type { ApiResponse } from "../../model/Api";
import type { Autor } from "../../model/Autor";
import api from "../../services/api";

export default function AutoresPage() {
  const [autores, setAutores] = useState<Autor[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const loadAutores = async () => {
    setLoading(true);
    setError("");
    try {
      const { data } = await api.get<ApiResponse<Autor[]>>("/autores");
      setAutores(data.data ?? []);
    } catch (err) {
      console.error(err);
      setError("Não foi possível carregar os autores.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadAutores();
  }, []);

  const handleDelete = async (id: number) => {
    const confirmed = window.confirm("Excluir este autor?");
    if (!confirmed) return;

    try {
      await api.delete(`/autores/${id}`);
      setAutores((current) => current.filter((a) => a.id !== id));
    } catch (err) {
      console.error(err);
      setError("Erro ao excluir autor.");
    }
  };

  return (
    <section className="card">
      <div className="actions">
        <div className="section-title">
          <h2>Autores</h2>
          <span className="pill">{autores.length}</span>
        </div>
        <Link className="btn primary" to="/autores/novo">
          Novo autor
        </Link>
      </div>

      {error && <p className="status">{error}</p>}

      <table className="table">
        <thead>
          <tr>
            <th>Nome</th>
            <th>Total de livros</th>
            <th style={{ width: 180 }}>Ações</th>
          </tr>
        </thead>
        <tbody>
          {loading ? (
            <tr>
              <td colSpan={3}>Carregando...</td>
            </tr>
          ) : autores.length ? (
            autores.map((autor) => (
              <tr key={autor.id}>
                <td>{autor.nome}</td>
                <td>{autor.totalLivros ?? 0}</td>
                <td>
                  <div style={{ display: "flex", gap: "0.5rem" }}>
                    <Link
                      to={`/autores/${autor.id}/editar`}
                      className="btn secondary"
                    >
                      Editar
                    </Link>
                    <button
                      className="btn danger"
                      onClick={() => handleDelete(autor.id)}
                    >
                      Excluir
                    </button>
                  </div>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={3}>Nenhum autor cadastrado.</td>
            </tr>
          )}
        </tbody>
      </table>
    </section>
  );
}

