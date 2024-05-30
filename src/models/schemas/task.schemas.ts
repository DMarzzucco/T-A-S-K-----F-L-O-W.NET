import { z } from "zod";

export const SchemaTask = z.object({
    title: z.string({
        required_error: "title is requerid"
    }),
    data: z.string().datetime().optional()
})