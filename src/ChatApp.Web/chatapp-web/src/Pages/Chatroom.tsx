import {useParams} from "react-router";
import {useCallback, useEffect, useState} from "react";

interface GetRoomDetailsResponse {
    name: string;
}

export default function Chatroom() {
    const {roomSlug} = useParams<{ roomSlug: string }>();
    const [isLoading, setIsLoading] = useState(false);
    const [error, setError] = useState<string>("");
    const [roomDetails, setRoomDetails] = useState<GetRoomDetailsResponse>(
        {
            name: ""
        });

    const fetchRoomDetails = useCallback(async () => {
        setIsLoading(true);

        try {
            const res = await fetch(`https://localhost:7073/api/ChatRoom/${roomSlug}`);

            if (res.status === 404) {
                setError("Room Not Found");
            } else {
                const data = await res.json();
                setRoomDetails(data)
            }
        } catch {
            setError("Unable to load room.");
        } finally {
            setIsLoading(false);
        }
    }, [roomSlug]);

    useEffect(() => {
        void fetchRoomDetails();
        }, [fetchRoomDetails]);
    
    return (

        <>
            {isLoading && (
                <p>Loading room...</p>
            )}

            {error && (
                <p>{error}</p>
            )}

            {!isLoading && (
                <h1>{roomDetails.name}</h1>
            )}
        </>
    )
}