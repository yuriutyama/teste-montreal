import { useEffect, useState } from "react";
import type { FormEvent } from "react";
import { Link, useNavigate, useParams } from "react-router-dom";
import type { ApiResponse } from "../../model/Api";
import type { Autor, AutorPayload } from "../../model/Autor";
import api from "../../services/api";

export default function AutorFormPage() {
  const { id } = useParams();
  const isEdit = Boolean(id);
  const navigate = useNavigate();

  const [form, setForm] = useState<AutorPayload>({ nome: "" });
  const [status, setStatus] = useState("");
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    if (!isEdit) return;

    const fetchAutor = async () => {
      try {
        const { data } = await api.get<ApiResponse<Autor>>(`/autores/${id}`);
        if (data.data) {
          setForm({ nome: data.data.nome });
        }
      } catch (err) {
        console.error(err);
        setStatus("Não foi possível carregar o autor.");
      }
    };

    fetchAutor();
  }, [id, isEdit]);

  const handleSubmit = async (event: FormEvent<HTMLFormElement>) => {
    event.preventDefault();
    setSaving(true);
    setStatus("");

    try {
      if (isEdit) {
        await api.put(`/autores/${id}`, form);
      } else {
        await api.post("/autores", form);
      }
      navigate("/autores");
    } catch (err) {
      console.error(err);
      setStatus("Erro ao salvar autor. Verifique os dados.");
    } finally {
      setSaving(false);
    }
  };

  return (
    <section className="card">
      <div className="actions">
        <h2>{isEdit ? "Editar autor" : "Novo autor"}</h2>
        <Link className="btn secondary" to="/autores">
          Voltar
        </Link>
      </div>

      <form onSubmit={handleSubmit} className="form-grid">
        <div className="field">
          <label htmlFor="nome">Nome</label>
          <input
            id="nome"
            name="nome"
            type="text"
            value={form.nome}
            onChange={(e) => setForm({ ...form, nome: e.target.value })}
            required
            maxLength={200}
          />
        </div>

        <div style={{ display: "flex", gap: "0.75rem" }}>
          <button className="btn primary" type="submit" disabled={saving}>
            {saving ? "Salvando..." : "Salvar"}
          </button>
          <Link to="/autores" className="btn secondary">
            Cancelar
          </Link>
        </div>
      </form>

      {status && <p className="status">{status}</p>}
    </section>
  );
}

