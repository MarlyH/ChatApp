import React, {useState} from 'react'

interface CreateRoomRequest {
    name: string;
    isPrivate: boolean;
}

export default function Landing() {
    const [showForm, setShowForm] = React.useState(false);
    const [form, setForm] = useState<CreateRoomRequest>({name: "", isPrivate: false});
    const [error, setError] = useState<string | null>(null);
    const [loading, setLoading] = useState<boolean>(false);

    const handleSubmit = async (event: React.FormEvent<HTMLFormElement>) => {
        event.preventDefault();
        setLoading(true);
        setError(null);

        try {
            const response = await fetch('https://localhost:7073/api/ChatRoom', {
                method: 'POST',
                headers: {
                    'Content-Type': 'application/json'
                },
                body: JSON.stringify(form)
            });
            const data = await response.json();

            if (!response.ok) {
                setError(data.message);
            } else {
                alert("success");
            }
        } catch {
            setError("Something went wrong");
        } finally {
            setLoading(false);
        }
    }

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {
        const target = event.target;
        const value = target.type === 'checkbox' ? target.checked : target.value;
        const name= target.name;
        setForm(prevState => ({ ...prevState, [name]: value }));
    }

    return (
        <div>
            <h1>Landing</h1>

            {!showForm ? (
                <>
                    <button
                        onClick={() => setShowForm(true)}
                        className="px-6 py-3 bg-teal-600 text-white font-semibold rounded-lg shadow-md hover:bg-teal-700 transition-colors duration-200 hover:cursor-pointer"
                    >
                        New Chatroom
                    </button>
                </>
            ) : (
                <>
                    {error !== null && (
                        <p className="text-red-500">{error}</p>
                    )}
                    <form
                        onSubmit={handleSubmit}
                        className="px-6 py-3 bg-teal-600 text-white font-semibold rounded-lg"
                    >
                        <label className="me-2">Name:</label>
                        <input type="text" id="name" name="name" value={form.name} onChange={handleChange}/>

                        <label className="mx-2">IsPrivate</label>
                        <input type="checkbox" id="isPrivate" name="isPrivate" checked={form.isPrivate} onChange={handleChange} />

                        <button type="submit" className="px-6 py-3 bg-teal-600 text-white font-semibold rounded-lg">Create</button>
                    </form>
                    {loading && (
                        <h2>Loading...</h2>
                    )}
                </>
            )}
        </div>
    )
}