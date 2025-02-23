#![feature(proc_macro_diagnostic)]
#![feature(proc_macro_span)]

extern crate proc_macro;

use proc_macro::TokenStream;
use quote::quote;
use syn::parse_macro_input;
use syn::spanned::Spanned;

#[proc_macro_derive(IterableEnum)]
pub fn make_iterable_enum(input: TokenStream) -> TokenStream {
    let ast = parse_macro_input!(input as syn::DeriveInput);

    let name = ast.ident;
    let result = match ast.data {
        syn::Data::Enum(data) => {
            // Loop through the enum variants so that we can generate the
            // iterator transitions

            let mut first = quote!{ None };
            let mut prev = None;
            let mut match_branches = vec![];

            for variant in data.variants {
                match variant.fields {
                    // Require all variants to be valueless
                    syn::Fields::Unit => {
                        let variant_identifier = variant.ident.clone();
                        match prev {
                            Some(p) => {
                                // If we have a predecessor, add a branch to the
                                // match from the predecessor to this variant
                                match_branches.push(quote! {
                                    Some(<#name>::#p) => Some(<#name>::#variant_identifier)
                                });
                            }
                            None => {
                                // If there is no predecessor, this should be
                                // the initial internal state, so that it's also
                                // the first emitted symbol
                                first = quote!{ Some(<#name>::#variant_identifier) };
                            }
                        }
                        prev = Some(variant.ident);
                    },
                    fields => {
                        // If this variant has an associated value, complain
                        variant.ident.span().unwrap()
                            .join(fields.span().unwrap())
                            .unwrap()
                            .error("IterableEnum can only be used with data-free enums")
                            .emit();

                        // Don't give multiple errors for the same enum.
                        // We could return here, but that means that no iterator
                        // will be produced, so the compiler will generate more
                        // errors where it's used. If we continue, it will
                        // produce something, so only the error that they need
                        // to action is displayed.
                        break;
                    }
                };
            }

            // Create a unique name for the state enum
            let state_enum_name_string = format!("__{}IteratorInternalState", name);
            let state_enum_name = syn::Ident::new(&state_enum_name_string, name.span());

            quote! {
                impl #name {
                    /// Get an iterator over the values of this enum
                    pub fn values() -> #state_enum_name {
                        #state_enum_name{ next: #first }
                    }
                }

                pub struct #state_enum_name {
                    /// The value to be returned next time next() is called.
                    /// Storing the next value means that we only need one
                    /// version of it, so we don't need to force Copy or Clone
                    /// to be implemented on the enum.
                    next: Option<#name>,
                }

                impl Iterator for #state_enum_name {
                    type Item = #name;

                    fn next(&mut self) -> Option<Self::Item> {
                        let val = self.next.take();
                        self.next = match &val {
                            // State transition table for the enum
                            #(#match_branches,)*
                            _ => None
                        };
                        val
                    }
                }
            }
        },
        syn::Data::Struct(data) => {
            data.struct_token.span.unwrap().join(name.span().unwrap()).unwrap()
                .error("IterableEnum can only be used on enums")
                .emit();
            quote!{}
        },
        syn::Data::Union(data) => {
            data.union_token.span.unwrap().join(name.span().unwrap()).unwrap()
                .error("IterableEnum can only be used on enums")
                .emit();
            quote!{}
        },
    };
    result.into()
}

trait IterableEnum { }

#[proc_macro_derive(TryFrom)]
pub fn make_enum_tryfrom_u32(input: TokenStream) -> TokenStream {
    let ast = parse_macro_input!(input as syn::DeriveInput);

    let name = ast.ident;
    let result = match ast.data {
        syn::Data::Enum(data) => {
            // Loop through the enum variants and generate the mapping

            let mut match_branches = vec![];

            for variant in data.variants {
                match variant.fields {
                    // Require all variants to be valueless
                    syn::Fields::Unit => {
                        let variant_identifier = variant.ident.clone();
                        match variant.discriminant {
                            Some((_, dis)) => match_branches.push(quote!{
                                #dis => Ok(<#name>::#variant_identifier)
                            }),
                            None => {
                                variant.span().unwrap()
                                    .error("TryFrom<u32> can only be derived if the discriminants are explicitly specified")
                                    .emit();
                                break;
                            }
                        }
                    },
                    fields => {
                        // If this variant has an associated value, complain
                        variant.ident.span().unwrap()
                            .join(fields.span().unwrap())
                            .unwrap()
                            .error("TryFrom<u32> can only be derived with data-free enums")
                            .emit();

                        // Don't give multiple errors for the same enum.
                        // We could return here, but that means that no iterator
                        // will be produced, so the compiler will generate more
                        // errors where it's used. If we continue, it will
                        // produce something, so only the error that they need
                        // to action is displayed.
                        break;
                    }
                };
            }

            // Create a unique name for the state enum
            //let error_enum_name_string = format!("{}FromU32Error", name);
            //let error_enum_name = syn::Ident::new(&error_enum_name_string, name.span());

            quote! {
                impl TryFrom<u32> for #name {
                    type Error = u32;

                    fn try_from(v: u32) -> ::core::result::Result<Self, u32> {
                        match v {
                            // State transition table for the enum
                            #(#match_branches,)*
                            v => Err(v)
                        }
                    }
                }
            }
        },
        syn::Data::Struct(data) => {
            data.struct_token.span.unwrap().join(name.span().unwrap()).unwrap()
                .error("TryFrom<u32> can only be derived on enums")
                .emit();
            quote!{}
        },
        syn::Data::Union(data) => {
            data.union_token.span.unwrap().join(name.span().unwrap()).unwrap()
                .error("TryFrom<u32> can only be derived on enums")
                .emit();
            quote!{}
        },
    };
    result.into()
}


