import { useEffect, useState } from "react";
import type { FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import type { ApiResponse } from "../../model/Api";
import type { Genero, GeneroPayload } from "../../model/Genero";
import api from "../../services/api";

export default function GeneroFormPage() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();

  const [form, setForm] = useState<GeneroPayload>({ descricao: "" });
  const [status, setStatus] = useState("");
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    if (!isEdit) return;

    const fetchGenero = async () => {
      try {
        const { data } = await api.get<ApiResponse<Genero>>(`/generos/${id}`);
        if (data.data) {
          setForm({ descricao: data.data.descricao });
        }
      } catch (err) {
        console.error(err);
        setStatus("Não foi possível carregar o gênero.");
      }
    };

    fetchGenero();
  }, [id, isEdit]);

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setSaving(true);
    setStatus("");

    try {
      if (isEdit) {
        await api.put(`/generos/${id}`, form);
      } else {
        await api.post("/generos", form);
      }
      navigate("/generos");
    } catch (err) {
      console.error(err);
      setStatus("Erro ao salvar gênero. Verifique os dados.");
    } finally {
      setSaving(false);
    }
  };

  return (
    <section className="card">
      <div className="actions">
        <h2>{isEdit ? "Editar gênero" : "Novo gênero"}</h2>
        <Link className="btn secondary" to="/generos">
          Voltar
        </Link>
      </div>

      <form onSubmit={handleSubmit} className="form-grid">
        <div className="field">
          <label htmlFor="descricao">Descrição</label>
          <input
            id="descricao"
            name="descricao"
            type="text"
            value={form.descricao}
            onChange={(e) => setForm({ ...form, descricao: e.target.value })}
            required
            maxLength={100}
          />
        </div>

        <div style={{ display: "flex", gap: "0.75rem" }}>
          <button className="btn primary" type="submit" disabled={saving}>
            {saving ? "Salvando..." : "Salvar"}
          </button>
          <Link to="/generos" className="btn secondary">
            Cancelar
          </Link>
        </div>
      </form>

      {status && <p className="status">{status}</p>}
    </section>
  );
}

