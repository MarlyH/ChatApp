import { useState } from "react";
import UsernameInput from "../Components/UsernameInput.tsx";
import EmailInput from "../Components/EmailInput.tsx";
import PasswordInput from "../Components/PasswordInput.tsx";

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
            const registerRes = await fetch("https://localhost:7073/api/user/register", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(form),
                credentials: "include"
            });

            if (!registerRes.ok) {
                const data = await registerRes.json();
                // flatten any validation errors returned by API and cast as string.
                const flattenedErrors = Object.values(data.errors).flat().map(e => String(e));
                setErrors(flattenedErrors);
            } else {
                //setForm({username: "", email: "", password: "", });
                // For now we are just automatically logging in. The actual intended flow is:
                // TODO: redirect to login page and send registration email to user
                const loginRes = await fetch('https://localhost:7073/api/User/login', {
                    method: 'POST',
                    headers: { "Content-Type": "application/json" },
                    body: JSON.stringify({
                        username: form.username,
                        password: form.password,
                        isPersistent: false
                    }),
                    credentials: "include"
                });
                if (loginRes.ok) {
                    alert("login successfully registered");
                } else if (loginRes.status === 403) {
                    setErrors(["Successfully registered but issue encountered logging in."])
                    // TODO: redirect to login screen for manual login.
                } else {
                    setErrors(["Something went wrong"]);
                }
            }
        } catch {
            setErrors(["Something went wrong"]);
        } finally {
            setLoading(false);
        }
    };

    return (
        <form onSubmit={handleSubmit}>
            <UsernameInput value={form.username} name={"username"} onChange={handleChange} />
            <EmailInput value={form.email} name={"email"} onChange={handleChange} />
            <PasswordInput value={form.password} name={"password"} onChange={handleChange} />
            <button className="btn btn-soft btn-primary" type="submit">
                {loading && (
                    <span className="loading loading-spinner"></span>
                )}
                Register
            </button>

            {errors.length > 0 && (
                <ul className={"text-red-700"}>
                    {errors.map((err, i) => <li key={i}>{err}</li>)}
                </ul>
            )}
        </form>
    );
}
