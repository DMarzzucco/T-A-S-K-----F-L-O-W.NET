#[cfg(test)]
mod tests {
    use macros::*;

    #[derive(IterableEnum, Debug, PartialEq)]
    enum AnEnum {
        A,
        B,
        C,
    }

    #[derive(IterableEnum, Debug, PartialEq)]
    enum AnotherEnum {
    }

    #[test]
    fn it_works() {
        let mut it = AnEnum::values();
        assert_eq!(it.next(), Some(AnEnum::A));
        assert_eq!(it.next(), Some(AnEnum::B));
        assert_eq!(it.next(), Some(AnEnum::C));
        assert_eq!(it.next(), None);
        assert_eq!(it.next(), None);
    }

    #[test]
    fn empty_enum() {
        let mut it = AnotherEnum::values();
        assert_eq!(it.next(), None);
    }
}
