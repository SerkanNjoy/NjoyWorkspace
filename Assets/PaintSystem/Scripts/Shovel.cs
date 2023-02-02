using UnityEngine;

public class Shovel : BaseTool
{
    [SerializeField] private Animation shovelAnimation;
    [SerializeField] private AnimationEventHandler eventHandler;
    [SerializeField] private GameObject shovelMaskPref;

    private ShovelMaskPool _shovelMaskPool;

    private Vector3 _lastUsedPosition;
    private int _lastUsedPositionCount;

    public override bool IsUsable
    {
        get
        {
            return true;
        }
    }

    private bool _ready;

    [ContextMenu("Init")]
    public override void Init()
    {
        _shovelMaskPool = new ShovelMaskPool(shovelMaskPref);
        _ready = true;
        _lastUsedPosition = Vector3.zero;
        _lastUsedPositionCount = 0;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Use();
        }
    }

    public override void Deselect()
    {
        
    }

    public override void Select()
    {
        _ready = true;
        _lastUsedPosition = Vector3.zero;
        _lastUsedPositionCount = 0;
    }

    public override void Use()
    {
        if (!_ready) return;

        Camera mainCam = Camera.main;

        transform.position = mainCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, -mainCam.transform.position.z));
        _ready = false;
        eventHandler.SetAnimationEvent(ShovelAction);
        shovelAnimation.Play("ShovelUseAnim");
    }

    private void ShovelAction()
    {
        if(Vector3.Distance(transform.position, _lastUsedPosition) <= 1f)
        {
            _lastUsedPositionCount++;
            if(_lastUsedPositionCount >= 3)
            {
                _lastUsedPositionCount = 0;
                _shovelMaskPool.SetMaskAtPoint(transform.position, 2.5f);
            }
        }
        else
        {
            _lastUsedPosition = transform.position;
            _lastUsedPositionCount = 1;
            _shovelMaskPool.SetMaskAtPoint(transform.position);
        }

        _ready = true;
    }
}