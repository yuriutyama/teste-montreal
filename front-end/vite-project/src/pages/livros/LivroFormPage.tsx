import { useEffect, useState } from "react";
import type { FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import type { ApiResponse } from "../../model/Api";
import type { Autor } from "../../model/Autor";
import type { Genero } from "../../model/Genero";
import type { Livro, LivroPayload } from "../../model/Livro";
import api from "../../services/api";

export default function LivroFormPage() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();

  const [form, setForm] = useState<LivroPayload>({
    titulo: "",
    autorId: 0,
    generoId: 0,
  });
  const [autores, setAutores] = useState<Autor[]>([]);
  const [generos, setGeneros] = useState<Genero[]>([]);
  const [status, setStatus] = useState("");
  const [saving, setSaving] = useState(false);

  const loadOptions = async () => {
    try {
      const [autoresRes, generosRes] = await Promise.all([
        api.get<ApiResponse<Autor[]>>("/autores"),
        api.get<ApiResponse<Genero[]>>("/generos"),
      ]);
      setAutores(autoresRes.data.data ?? []);
      setGeneros(generosRes.data.data ?? []);
    } catch (err) {
      console.error(err);
      setStatus("Não foi possível carregar autores ou gêneros.");
    }
  };

  const loadLivro = async () => {
    if (!isEdit) return;
    try {
      const { data } = await api.get<ApiResponse<Livro>>(`/livros/${id}`);
      if (data.data) {
        setForm({
          titulo: data.data.titulo,
          autorId: data.data.autorId,
          generoId: data.data.generoId,
        });
      }
    } catch (err) {
      console.error(err);
      setStatus("Não foi possível carregar o livro.");
    }
  };

  useEffect(() => {
    loadOptions();
    loadLivro();
  }, [id, isEdit]);

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setSaving(true);
    setStatus("");

    if (!form.autorId || !form.generoId) {
      setStatus("Selecione um autor e um gênero.");
      setSaving(false);
      return;
    }

    try {
      if (isEdit) {
        await api.put(`/livros/${id}`, form);
      } else {
        await api.post("/livros", form);
      }
      navigate("/livros");
    } catch (err) {
      console.error(err);
      setStatus("Erro ao salvar livro. Verifique os dados.");
    } finally {
      setSaving(false);
    }
  };

  return (
    <section className="card">
      <div className="actions">
        <h2>{isEdit ? "Editar livro" : "Novo livro"}</h2>
        <Link className="btn secondary" to="/livros">
          Voltar
        </Link>
      </div>

      <form onSubmit={handleSubmit} className="form-grid">
        <div className="field">
          <label htmlFor="titulo">Título</label>
          <input
            id="titulo"
            name="titulo"
            type="text"
            value={form.titulo}
            onChange={(e) => setForm({ ...form, titulo: e.target.value })}
            required
            maxLength={300}
          />
        </div>

        <div className="field">
          <label htmlFor="autorId">Autor</label>
          <select
            id="autorId"
            name="autorId"
            value={form.autorId}
            onChange={(e) =>
              setForm({ ...form, autorId: Number(e.target.value) })
            }
            required
          >
            <option value={0}>Selecione um autor</option>
            {autores.map((autor) => (
              <option key={autor.id} value={autor.id}>
                {autor.nome}
              </option>
            ))}
          </select>
        </div>

        <div className="field">
          <label htmlFor="generoId">Gênero</label>
          <select
            id="generoId"
            name="generoId"
            value={form.generoId}
            onChange={(e) =>
              setForm({ ...form, generoId: Number(e.target.value) })
            }
            required
          >
            <option value={0}>Selecione um gênero</option>
            {generos.map((genero) => (
              <option key={genero.id} value={genero.id}>
                {genero.descricao}
              </option>
            ))}
          </select>
        </div>

        <div style={{ display: "flex", gap: "0.75rem" }}>
          <button className="btn primary" type="submit" disabled={saving}>
            {saving ? "Salvando..." : "Salvar"}
          </button>
          <Link to="/livros" className="btn secondary">
            Cancelar
          </Link>
        </div>
      </form>

      {status && <p className="status">{status}</p>}
    </section>
  );
}

