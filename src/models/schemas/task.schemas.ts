import { z } from "zod";

export const SchemaTask = z.object({
    title: z.string({ required_error: "title is requerid" }),
    descrption: z.string({ required_error: "description is required" }),
    data: z.string().datetime().optional()
})