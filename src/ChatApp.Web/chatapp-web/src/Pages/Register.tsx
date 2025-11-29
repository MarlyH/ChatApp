import { useState } from "react";

interface RegisterRequest {
    username: string;
    email: string;
    password: string;
}

export default function Register() {
    const [form, setForm] = useState<RegisterRequest>({
        username: "",
        email: "",
        password: "",
    });

    const [errors, setErrors] = useState<string[]>([]);
    const [loading, setLoading] = useState(false);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        setForm({ ...form, [e.target.name]: e.target.value });
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setErrors([]);
        setLoading(true);

        try {
            const res = await fetch("https://localhost:7073/api/user", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(form),
            });

            if (!res.ok) {
                const data = await res.json();
                // flatten any validation errors returned by API and cast as string.
                const flattenedErrors = Object.values(data.errors).flat().map(e => String(e));
                setErrors(flattenedErrors);
            } else {
                alert("User successfully registered");
                setForm({username: "", email: "", password: "", });
            }
        } catch {
            setErrors(["Something went wrong"]);
        } finally {
            setLoading(false);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <input name="username" value={form.username} onChange={handleChange} placeholder="Username" required />
            <input name="email" value={form.email} onChange={handleChange} placeholder="Email" required />
            <input type="password" name="password" value={form.password} onChange={handleChange} placeholder="Password" required />
            <button type="submit">Register</button>

            {errors.length > 0 && (
                <ul className={"text-red-700"}>
                    {errors.map((err, i) => <li key={i}>{err}</li>)}
                </ul>
            )}
            {loading && (
                <p>Loading...</p>
            )}
        </form>
    );
}
