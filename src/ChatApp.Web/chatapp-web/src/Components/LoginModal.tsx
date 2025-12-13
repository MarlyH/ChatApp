import UsernameInput from "./UsernameInput.tsx";
import PasswordInput from "./PasswordInput.tsx";
import {useState} from "react";
import PersistenceInput from "./PersistenceInput.tsx";

interface LoginRequest {
    username: string;
    password: string;
    isPersistent: boolean;
}

export default function LoginModal() {
    const [form, setForm] = useState<LoginRequest>({
        username: "",
        password: "",
        isPersistent: false
    });

    const [errors, setErrors] = useState<string[]>([]);
    const [loading, setLoading] = useState(false);

    const handleChange = (e: React.ChangeEvent<HTMLInputElement>) => {
        if (e.target.type === "checkbox") {
            setForm({...form, [e.target.name]: e.target.checked});
        } else {
            setForm({ ...form, [e.target.name]: e.target.value });
        }
    };

    const handleSubmit = async (e: React.FormEvent) => {
        e.preventDefault();
        setErrors([]);
        setLoading(true);

        try {
            console.log(form);
            const loginRes = await fetch("https://localhost:7073/api/user/login", {
                method: "POST",
                headers: { "Content-Type": "application/json" },
                body: JSON.stringify(form),
                credentials: "include"
            });

            if (!loginRes.ok) {
                const data = await loginRes.json();
                // flatten any validation errors returned by API and cast as string.
                const flattenedErrors = Object.values(data.errors).flat().map(e => String(e));
                setErrors(flattenedErrors);
            } else {
                // login success, redirect
                alert("successfully logged in!");
            }
        } catch {
            setErrors(["Something went wrong"]);
        } finally {
            setLoading(false);
        }
    };

    return (
        <>
            {/* You can open the modal using document.getElementById('ID').showModal() method */}
            <button
                className="btn"
                onClick={() => (document.getElementById('my_modal_4') as HTMLDialogElement)?.showModal()}
            >
                Log In
            </button>

            <dialog id="my_modal_4" className="modal">
                <div className="modal-box">
                    <form method="dialog">
                        {/* if there is a button in form, it will close the modal */}
                        <button className="btn btn-sm btn-circle btn-ghost absolute right-2 top-2">✕</button>
                    </form>

                    <div className="flex flex-col gap-4">
                        <h2 className="font-bold text-xl">Log In</h2>

                        <form onSubmit={handleSubmit} className="gap-2">
                            <UsernameInput value={form.username} name={"username"} onChange={handleChange} />
                            <PasswordInput value={form.password} name={"password"} onChange={handleChange} />
                            <PersistenceInput checked={form.isPersistent} name={"isPersistent"} handleChange={handleChange} />
                            <button className="btn btn-soft btn-primary mt-4" type="submit">
                                {loading && (
                                    <span className="loading loading-spinner"></span>
                                )}
                                Log In
                            </button>
                        </form>

                        {errors.length > 0 && (
                            <ul className={"text-red-700"}>
                                {errors.map((err, i) => <li key={i}>{err}</li>)}
                            </ul>
                        )}
                    </div>
                </div>
            </dialog>
        </>
    )
}