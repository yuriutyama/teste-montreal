import { useEffect, useState } from "react";
import { Link } from "react-router-dom";
import type { ApiResponse } from "../../model/Api";
import type { Genero } from "../../model/Genero";
import api from "../../services/api";

export default function GenerosPage() {
  const [generos, setGeneros] = useState<Genero[]>([]);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState("");

  const loadGeneros = async () => {
    setLoading(true);
    setError("");
    try {
      const { data } = await api.get<ApiResponse<Genero[]>>("/generos");
      setGeneros(data.data ?? []);
    } catch (err) {
      console.error(err);
      setError("Não foi possível carregar os gêneros.");
    } finally {
      setLoading(false);
    }
  };

  useEffect(() => {
    loadGeneros();
  }, []);

  const handleDelete = async (id: number) => {
    const confirmed = window.confirm("Excluir este gênero?");
    if (!confirmed) return;

    try {
      await api.delete(`/generos/${id}`);
      setGeneros((current) => current.filter((g) => g.id !== id));
    } catch (err) {
      console.error(err);
      setError("Erro ao excluir gênero.");
    }
  };

  return (
    <section className="card">
      <div className="actions">
        <div className="section-title">
          <h2>Gêneros</h2>
          <span className="pill">{generos.length}</span>
        </div>
        <Link className="btn primary" to="/generos/novo">
          Novo gênero
        </Link>
      </div>

      {error && <p className="status">{error}</p>}

      <table className="table">
        <thead>
          <tr>
            <th>Descrição</th>
            <th>Total de livros</th>
            <th style={{ width: 180 }}>Ações</th>
          </tr>
        </thead>
        <tbody>
          {loading ? (
            <tr>
              <td colSpan={3}>Carregando...</td>
            </tr>
          ) : generos.length ? (
            generos.map((genero) => (
              <tr key={genero.id}>
                <td>{genero.descricao}</td>
                <td>{genero.totalLivros ?? 0}</td>
                <td>
                  <div style={{ display: "flex", gap: "0.5rem" }}>
                    <Link
                      to={`/generos/${genero.id}/editar`}
                      className="btn secondary"
                    >
                      Editar
                    </Link>
                    <button
                      className="btn danger"
                      onClick={() => handleDelete(genero.id)}
                    >
                      Excluir
                    </button>
                  </div>
                </td>
              </tr>
            ))
          ) : (
            <tr>
              <td colSpan={3}>Nenhum gênero cadastrado.</td>
            </tr>
          )}
        </tbody>
      </table>
    </section>
  );
}

