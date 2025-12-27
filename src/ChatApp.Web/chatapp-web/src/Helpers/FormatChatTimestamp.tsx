export default function formatChatTimestamp(isoUtc: Date | string): string {
    // raw ISO input example: 2025-12-22T03:59:49.797017Z
    const date: Date = new Date(isoUtc);
    // "2025-12-22T03:59:49.797Z"

    const options: Intl.DateTimeFormatOptions = {
        year: 'numeric',
        month: 'short',
        day: 'numeric',
        hour: 'numeric',
        minute: '2-digit',
        hour12: true
    };
    // "22 Dec 2025, 4:59 PM"

    if (isToday(date)) {
        const todayOptions: Intl.DateTimeFormatOptions = {
            hour: 'numeric',
            minute: '2-digit',
            hour12: true
        }

        // "4:59 PM"
        return date.toLocaleString(undefined, todayOptions);
    }

    return date.toLocaleString(undefined, options);
}

function isToday(date: Date): boolean {
    const today = new Date();

    return (
        date.getFullYear() === today.getFullYear() &&
        date.getMonth() === today.getMonth() &&
        date.getDate() === today.getDate()
    );
}

const TIME_BREAK_MINUTES = 30;

export function isTimeBreak(prev: Date | null, current: Date): boolean {
    if (!prev) return true; // always show timestamp for the first message
    const diffMs = current.getTime() - prev.getTime();
    const diffMinutes = diffMs / (1000 * 60);
    return diffMinutes >= TIME_BREAK_MINUTES;
}

