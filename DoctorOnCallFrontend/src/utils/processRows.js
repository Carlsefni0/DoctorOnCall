export default  function processRows(rows) {
    return rows.map((row) => {
        const date = new Date(row.requestedDateTime);
        return {
            ...row,
            requestedDate: new Intl.DateTimeFormat("en-GB", {
                day: "numeric",
                month: "long",
                year: "numeric",
            }).format(date),
            requestedTime: date.toLocaleTimeString("en-GB", {
                hour: "2-digit",
                minute: "2-digit",
                second: "2-digit",
            }),
        };
    });
};
