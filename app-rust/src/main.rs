mod cli;

fn main() {
    let args: Vec<String> = std::env::args().collect();
    print!("{}", args[0]);

    let mut app = cli::Cli::new();
    app.run();
}
