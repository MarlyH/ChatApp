import {useParams} from "react-router";

export default function Chatroom() {
    const {roomSlug} = useParams<{ roomSlug: string }>();

    return (

        <>
            <h1>{roomSlug}</h1>
        </>
    )
}