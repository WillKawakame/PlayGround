public class StateFactory {

    Object _context;

    public StateFactory( Object currentContext ) {
        _context = currentContext;
    }

    public Base State_() {
        return new State();
    } 
    public Base State1_(){
        return new State1();
    } 
}
