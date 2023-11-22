namespace IWent.Api.Parameters;

/// <summary>
/// Contains pagination parameters for controllers.
/// </summary>
public class PaginationParameters
{
    private const int DefaultPageSize = 10;
    private const int MaxPageSize = 50;
    private readonly int _page = 1;
    private readonly int _size = DefaultPageSize;

    public PaginationParameters()
        : this(page: 1, size: 10)
    {
    }

    public PaginationParameters(int page, int size)
    {
        Page = page;
        Size = size;
    }

    public int Page 
    {
        get => _page;
        init
        {
            if (value < 1)
            {
                _page = 1;
                return;
            }

            _page = value;
        }
    }

    public int Size
    {
        get => _size;
        init
        {
            if (value < 1 || value > MaxPageSize)
            {
                _page = DefaultPageSize;
                return;
            }

            _size = value;
        }
    }
}
